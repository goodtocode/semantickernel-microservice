#-----------------------------------------------------------------------
# Remove-LandingZone [-TenantId [<Guid>]] [-SubscriptionId [<Guid>]]
#
# Example: .\Remove-LandingZone -TenantId -SubscriptionId -ResourceGroup -KeyVault -StorageAccount -Workspace
#-----------------------------------------------------------------------

# ***
# *** Parameters
# ***
param
(
	[string] $TenantId=$(throw '-TenantId is a required parameter. (00000000-0000-0000-0000-000000000000)'),
    [string] $SubscriptionId=$(throw '-TenantId is a required parameter. (00000000-0000-0000-0000-000000000000)'),
	[string] $ResourceGroup=$(throw '-ResourceGroup is a required parameter. (rg-PRODUCT-ENVIRONMENT-001)'),
    [string] $KeyVault=$(throw '-KeyVault is a required parameter. (kv-PRODUCT-ENVIRONMENT-001)'),
    [string] $StorageAccount=$(throw '-StorageAccount is a required parameter. (stPRODUCTENVIRONMENT001)'),
    [string] $Workspace=$(throw '-Workspace is a required parameter. (work-PRODUCT-ENVIRONMENT-001)')
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

# ***
# *** Execute
# ***
Remove-AzStorageAccount -ResourceGroupName $ResourceGroup -AccountName $StorageAccount
Remove-AzKeyVault -VaultName $KeyVault -PassThru
Remove-AzOperationalInsightsWorkspace -ResourceGroupName $ResourceGroup -Name $Workspace -ForceDelete