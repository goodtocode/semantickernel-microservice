#-----------------------------------------------------------------------
# Remove-SQLServer [-Path [<String>]] [-VersionToReplace [<String>]]
#
# Example: .\Remove-StorageAccount -TenantId -SubscriptionId -ResourceGroup -StorageAccount
#-----------------------------------------------------------------------

# ***
# *** Parameters
# ***
param
(
	[string] $TenantId=$(throw '-TenantId is a required parameter. (00000000-0000-0000-0000-000000000000)'),
    [string] $SubscriptionId=$(throw '-TenantId is a required parameter. (00000000-0000-0000-0000-000000000000)'),
	[string] $ResourceGroup=$(throw '-ResourceGroup is a required parameter. (rg-PRODUCT-ENVIRONMENT-001)'),
    [string] $SQLServer=$(throw '-SQLServer is a required parameter. (sql-PRODUCT-ENVIRONMENT-001)')
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
Import-Module "./System.psm1"
Install-Module -Name Az.Accounts -AllowClobber -Scope CurrentUser
Install-Module -Name Az.Sql -AllowClobber -Scope CurrentUser

# ***
# *** Locals
# ***

# ***
# *** Auth
# ***
Write-Host "*** Auth ***"

Connect-AzAccount -Tenant $TenantId -Subscription $SubscriptionId

# ***
# *** Execute
# ***
Get-AzSqlServer -ResourceGroupName $ResourceGroup
Remove-AzSqlServer -ResourceGroupName $ResourceGroup -AccountName $StorageAccount