#-----------------------------------------------------------------------
# Set-AzKeyVaultPolicy [-Name [<String>]] [-ObjectId [<Guid>]] [-SecretPermissions [<string>]]
#
# Example: .\Set-AzKeyVaultPolicy -Name kv-PRODUCT-ENVIRONMENT-001 -ObjectId 00000000-0000-0000-0000-000000000000 -SecretPermissions list get
# CLI: 
#   az keyvault set-policy --name "<KeyVaultName>" --spn <ClientId> --secret-permissions list get
#   az keyvault update --name "<KeyVaultName>" --resource-group "<ResourceGroupName>" --enabled-for-deployment "true"
#-----------------------------------------------------------------------

# ***
# *** Parameters
# ***
param
(
    [string] $Name=$(throw '-Name is a required parameter. (kv-PRODUCT-ENVIRONMENT-001)'),
    [string] $ObjectId=$(throw '-ObjectId is a required parameter. (00000000-0000-0000-0000-000000000000)'),
    [string] $SecretPermissions='list get'
)

# ***
# *** Initialize
# ***
if ($IsWindows) { Set-ExecutionPolicy Unrestricted -Scope Process -Force }
$VerbosePreference = 'SilentlyContinue' #'Continue'
[String]$ThisScript = $MyInvocation.MyCommand.Path
[String]$ThisDir = Split-Path $ThisScript
[DateTime]$Now = Get-Date
Set-Location $ThisDir # Ensure our location is correct, so we can use relative paths
Write-Host "*****************************"
Write-Host "*** Starting: $ThisScript on $Now"
Write-Host "*****************************"
# Imports
Import-Module "../System.psm1"
Install-Module -Name Az.Accounts -AllowClobber -Scope CurrentUser
Install-Module -Name Az.Resources -AllowClobber -Scope CurrentUser

# ***
# *** Auth
# ***
Write-Host "*** Auth ***"
Connect-AzAccount -Tenant $TenantId -Subscription $SubscriptionId

Set-AzKeyVaultAccessPolicy -VaultName $Name -ObjectId $ObjectId -PermissionsToSecrets $SecretPermissions -EnabledForDeployment