# ============================================================================
# Script Name:   Set-WebAppUserSecrets.ps1
# Description:   Reads an existing Web App Registration and sets .NET user-secrets.
# -----------------------------------------------------------------------------
# Example CLI Usage:
#   pwsh -File ./Set-WebAppUserSecrets.ps1 `
#       -TenantId "<your-tenant-id>" `
#       -WebAppRegistrationName "myproduct-web-dev-001" `
#       -WebProjectPath "../../src/Presentation.Blazor"
# -----------------------------------------------------------------------------
# Notes:
#   - Requires Azure PowerShell modules (Az.Accounts, Microsoft.Graph.Applications)
#   - Ensure you are authenticated: Connect-AzAccount
#   - This script does NOT create or modify app registrations.
# ============================================================================

param(
    [string]$TenantId,
    [string]$WebAppRegistrationName,
    [string]$WebProjectPath
)

function Install-Prerequisites {
    $modules = @("Az.Accounts", "Microsoft.Graph.Applications")
    foreach ($module in $modules) {
        if (-not (Get-Module -ListAvailable -Name $module)) {
            Write-Host "Installing PowerShell module: $module"
            Install-Module $module -Scope CurrentUser -Force
        }
    }
    Import-Module Az.Accounts -ErrorAction Stop
    Import-Module Microsoft.Graph.Applications -ErrorAction Stop
}

function New-Auth {
    param([string]$TenantId)
    $azContext = Get-AzContext -ErrorAction SilentlyContinue
    if (-not $azContext -or $azContext.Tenant.Id -ne $TenantId) {
        Connect-AzAccount -Tenant $TenantId | Out-Null
    }
    $mgContext = $null
    try { $mgContext = Get-MgContext -ErrorAction Stop } catch {}
    if (-not $mgContext -or $mgContext.TenantId -ne $TenantId -or -not $mgContext.Account) {
        Connect-MgGraph -TenantId $TenantId -Scopes "Application.Read.All" | Out-Null
    }
}

function Set-ProjectUserSecrets {
    param([string]$ProjectPath, [hashtable]$Secrets)
    if (Test-Path $ProjectPath) {
        Push-Location $ProjectPath
        dotnet user-secrets init
        foreach ($key in $Secrets.Keys) {
            dotnet user-secrets set $key $Secrets[$key]
        }
        Pop-Location
        Write-Host "Secrets set for $ProjectPath"
    } else {
        Write-Warning "*** Project path '$ProjectPath' not found. Skipping dotnet user-secrets. ***"
    }
}

Install-Prerequisites
New-Auth -TenantId $TenantId

$webApp = Get-MgApplication -Filter "displayName eq '$WebAppRegistrationName'" -ErrorAction Stop
if (-not $webApp) {
    Write-Error "Web app registration '$WebAppRegistrationName' not found."
    exit 1
}

$secrets = Get-MgApplicationPassword -ApplicationId $webApp.Id
$clientSecret = $secrets | Select-Object -First 1 -ExpandProperty SecretText

$webSecrets = @{
    "EntraExternalId:Instance"          = "https://login.microsoftonline.com"
    "EntraExternalId:TenantId"          = $TenantId
    "EntraExternalId:ClientId"          = $webApp.AppId
    "EntraExternalId:ValidateAuthority" = "true"
}
if ($clientSecret) {
    $webSecrets["EntraExternalId:ClientSecret"] = $clientSecret
}
Set-ProjectUserSecrets -ProjectPath $WebProjectPath -Secrets $webSecrets