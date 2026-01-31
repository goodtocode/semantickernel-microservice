
# ============================================================================
# Script Name:   New-EntraAppRegistrationSecret.ps1
# Description:   Creates a new client secret for an existing Entra External ID App Registration and sets it in dotnet user-secrets for a project.
# -----------------------------------------------------------------------------
# Example CLI Usage:
#   pwsh -File ./New-EntraAppRegistrationSecret.ps1 -TenantId "<your-tenant-id>" -AppRegistrationName "<app-registration-name>" -DotnetProjectPath "<path-to-project>"
# -----------------------------------------------------------------------------
# Notes:
#   - Requires PowerShell modules: Az.Accounts, Microsoft.Graph.Applications
#   - Ensure you are authenticated: Connect-AzAccount and Connect-MgGraph
# ============================================================================
param(
    [string]$TenantId,
    [string]$AppRegistrationName,
    [string]$DotnetProjectPath,
    [int]$SecretMonths = 24,
    [string]$DotNetVersion = "10"
)

# Step 1: Install prerequisites (dotnet sdk, modules)
Write-Host "Checking prerequisites..."

# Check and install .NET SDK
$dotnetInstalled = & dotnet --list-sdks | Select-String "^$DotNetVersion\."
if (-not $dotnetInstalled) {
	Write-Host ".NET SDK $DotNetVersion not found. Installing via winget..."
	winget install --id Microsoft.DotNet.SDK.$DotNetVersion -e --silent
} else {
	Write-Host ".NET SDK $DotNetVersion is already installed."
}

# Check and install PowerShell modules
$modules = @("Az.Accounts", "Az.Resources", "Microsoft.Graph.Applications")
foreach ($module in $modules) {
	if (-not (Get-Module -ListAvailable -Name $module)) {
		Write-Host "Installing PowerShell module: $module"
		Install-Module $module -Scope CurrentUser -Force
	} else {
		Write-Host "PowerShell module $module is already installed."
	}
}

# Step 2: Login to Azure and set EEID tenant
Write-Host "Checking Azure authentication..."
$azContext = Get-AzContext -ErrorAction SilentlyContinue
if ($azContext -and $azContext.Tenant.Id -eq $TenantId) {
	Write-Host "Already authenticated to Azure tenant $TenantId."
} else {
	Write-Host "Logging into Azure..."
	Connect-AzAccount -Tenant $TenantId | Out-Null
	Write-Host "Logged in to Azure tenant $TenantId."
}

# Check for Microsoft Graph authentication
$mgContext = $null
try {
    $mgContext = Get-MgContext -ErrorAction Stop
} catch {}
if (-not $mgContext -or $mgContext.TenantId -ne $TenantId -or -not $mgContext.Account) {
    Write-Host "Connecting to Microsoft Graph..."
    Connect-MgGraph -TenantId $TenantId -Scopes "Application.ReadWrite.All","Directory.ReadWrite.All" | Out-Null
    $mgContext = Get-MgContext -ErrorAction Stop
}

# Find the app registration
$app = Get-MgApplication -Filter "displayName eq '$AppRegistrationName'" -ErrorAction SilentlyContinue
if (-not $app) {
    Write-Error "FATAL: App registration '$AppRegistrationName' not found. Cannot create secret."
    exit 1
}

# Create new client secret

# Create password credential object for secret expiration
$endDate = (Get-Date).AddMonths($SecretMonths)
$passwordCredential = @{ EndDateTime = $endDate }
$secretObj = Add-MgApplicationPassword -ApplicationId $app.Id -PasswordCredential $passwordCredential
if (-not $secretObj -or -not $secretObj.SecretText) {
    Write-Error "FATAL: Failed to create client secret."
    exit 1
}
$secret = $secretObj.SecretText
Write-Host "Created new client secret for app registration '$AppRegistrationName'."

# Set secret in dotnet user-secrets if project path exists
if (Test-Path $DotnetProjectPath) {
    Write-Host "Setting EntraExternalId:ClientSecret in dotnet user-secrets for $DotnetProjectPath"
    Push-Location $DotnetProjectPath
    dotnet user-secrets init
    dotnet user-secrets set "EntraExternalId:ClientSecret" $secret
    Pop-Location
    Write-Host "Secret set in dotnet user-secrets."
} else {
    Write-Warning "*** CRITICAL: Project path '$DotnetProjectPath' not found. Skipping dotnet user-secrets. Secret was created in Entra ID. ***"
}

