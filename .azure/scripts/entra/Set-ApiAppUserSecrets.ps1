# ============================================================================
# Script Name:   Set-ApiAppUserSecrets.ps1
# Description:   Reads an existing API App Registration and sets .NET user-secrets.
# -----------------------------------------------------------------------------
# Example CLI Usage:
#   pwsh -File ./Set-ApiAppUserSecrets.ps1 `
#       -TenantId "<your-tenant-id>" `
#       -ApiAppRegistrationName "myproduct-api-dev-001" `
#       -ApiProjectPath "../../src/Presentation.WebApi"
# -----------------------------------------------------------------------------
# Notes:
#   - Requires Azure PowerShell modules (Az.Accounts, Microsoft.Graph.Applications)
#   - Ensure you are authenticated: Connect-AzAccount
#   - This script does NOT create or modify app registrations.
# ============================================================================

param(
    [string]$TenantId,
    [string]$ApiAppRegistrationName,
    [string]$ApiProjectPath
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

$apiApp = Get-MgApplication -Filter "displayName eq '$ApiAppRegistrationName'" -ErrorAction Stop
if (-not $apiApp) {
    Write-Error "API app registration '$ApiAppRegistrationName' not found."
    exit 1
}

$apiSecrets = @{
    "EntraExternalId:Instance"          = "https://login.microsoftonline.com"
    "EntraExternalId:TenantId"          = $TenantId
    "EntraExternalId:ClientId"          = $apiApp.AppId
    "EntraExternalId:ValidateAuthority" = "true"
}
Set-ProjectUserSecrets -ProjectPath $ApiProjectPath -Secrets $apiSecrets