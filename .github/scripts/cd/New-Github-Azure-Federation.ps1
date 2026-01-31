####################################################################################
# To execute
#   1. Run Powershell as ADMINistrator
#   2. In powershell, set security polilcy for this script: 
#      Set-ExecutionPolicy Unrestricted -Scope Process -Force
#   3. Change directory to the script folder:
#      CD C:\Scripts (wherever your script is)
#   4. In powershell, run script: 
#      .\New-AzureGitHubFederation.ps1 -SubscriptionId 12343dac-0e69-436a-866b-456727dd3579 -PrincipalName myco-github-devtest-001 -Organization mygithuborg -Repository mygithubrepo -Environment development
####################################################################################

param (
    [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[guid]$SubscriptionId = $(throw '-SubscriptionId is a required parameter.'), #12343dac-0e69-436a-866b-456727dd3579
    [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[string]$PrincipalName = $(throw '-PrincipalName is a required parameter.'), #Example: COMPANY-SUB_OR_PRODUCTLINE-github-001
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[string]$Organization = $(throw '-Organization is a required parameter.'), #GitHub Organization Name
    [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[string]$Repository = $(throw '-Repository is a required parameter.'), #GitHub Repository Name
    [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
 	[string]$Environment = $(throw '-Environment is a required parameter.') #GitHub Repository Environment: development, production
    )
####################################################################################
Set-ExecutionPolicy Unrestricted -Scope Process -Force
$VerbosePreference = 'SilentlyContinue' # 'SilentlyContinue' # 'Continue'
[String]$ThisScript = $MyInvocation.MyCommand.Path
[String]$ThisDir = Split-Path $ThisScript
Set-Location $ThisDir # Ensure our location is correct, so we can use relative paths
Write-Host "*****************************"
Write-Host "*** Starting: $ThisScript On: $(Get-Date)"
Write-Host "*****************************"
####################################################################################
# Install required modules
Install-Module Az.Accounts,Az.Resources -Scope CurrentUser -Force

# Login to Azure
Connect-AzAccount -SubscriptionId $SubscriptionId -UseDeviceAuthentication

# Get App Registration object (Application object)
$app = Get-AzADApplication -DisplayName $PrincipalName
if (-not $app) {
    $app = New-AzADApplication -DisplayName $PrincipalName
}
Write-Host "App Registration (Client) Id: $($app.AppId)"
$clientId = $app.AppId
$appObjectId = $app.Id

# Create Service Principal and assign role
$sp = Get-AzADServicePrincipal -DisplayName $PrincipalName
if (-not $sp) {
    $sp = New-AzADServicePrincipal -ApplicationId $clientId
}
Write-Host "Service Principal Id: $($sp.Id)"
$spObjectId = $sp.Id
New-AzRoleAssignment -ObjectId $spObjectId -RoleDefinitionName Contributor -Scope "/subscriptions/$SubscriptionId"

$tenantId = (Get-AzContext).Subscription.TenantId

# Create new App Registration Federated Credentials for the GitHub operations
$subjectRepo = "repo:" + $Organization + "/" + $Repository + ":environment:" + $Environment
New-AzADAppFederatedCredential -ApplicationObjectId $appObjectId -Audience api://AzureADTokenExchange -Issuer 'https://token.actions.githubusercontent.com' -Name "$PrincipalName-repo" -Subject "$subjectRepo"
$subjectRepoMain = "repo:" + $Organization + "/" + $Repository + ":ref:refs/heads/main"
New-AzADAppFederatedCredential -ApplicationObjectId $appObjectId -Audience api://AzureADTokenExchange -Issuer 'https://token.actions.githubusercontent.com' -Name "$PrincipalName-main" -Subject "$subjectRepoMain"
$subjectRepoPR = "repo:" + $Organization + "/" + $Repository + ":pull_request"
New-AzADAppFederatedCredential -ApplicationObjectId $appObjectId -Audience api://AzureADTokenExchange -Issuer 'https://token.actions.githubusercontent.com' -Name "$PrincipalName-PR" -Subject "$subjectRepoPR"

Write-Host "AZURE_TENANT_ID: $tenantId"
Write-Host "AZURE_SUBSCRIPTION_ID: $SubscriptionId"
Write-Host "AZURE_CLIENT_ID: $clientId"