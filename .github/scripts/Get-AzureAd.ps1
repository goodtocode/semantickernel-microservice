####################################################################################
# To execute
#   1. Run Powershell as ADMINistrator
#   2. In powershell, set security polilcy for this script: 
#      Set-ExecutionPolicy Unrestricted -Scope Process -Force
#   3. Change directory to the script folder:
#      CD C:\Temp (wherever your script is)
#   4. In powershell, run script: 
#      .\Get-AzureAd.ps1
####################################################################################

param (
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
# ***
# *** Imports
# ***
Write-Host "*** Imports ***"
Import-Module "./System.psm1"
Install-Module -Name Az -AllowClobber -Scope CurrentUser # All
Install-Module -Name AzureAD -AllowClobber -Scope CurrentUser
#Install-Module -Name Az.Accounts -AllowClobber -Scope CurrentUser
#Install-Module -Name Az.Billing -AllowClobber -Scope CurrentUser
#Install-Module -Name Az.Resources -AllowClobber -Scope CurrentUser

# ***
# *** Auth
# ***
Write-Host "*** Auth ***"
Connect-AzureAD -TenantId "00000000-0000-0000-0000-000000000000"

# Find service principal object ID
Write-Host "*** Get-AzureADServicePrincipal ***"
$oid = $(Get-AzureADServicePrincipal -Filter "AppId eq '00000000-0000-0000-0000-000000000000'").ObjectId
Write-host "$oid"
#$(Get-AzureADServicePrincipal -Filter "DisplayName eq 'testapp'").ObjectId
# Find user object ID
#$(Get-AzureADUser -Filter "UserPrincipalName eq 'myuser@contoso.com'").ObjectId
# Find a security group object ID
#$(Get-AzureADGroup -Filter "DisplayName eq 'mygroup'").ObjectId