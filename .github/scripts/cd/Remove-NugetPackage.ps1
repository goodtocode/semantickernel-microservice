<#
.SYNOPSIS
Deletes all versions of a specified NuGet package from a NuGet feed.
.DESCRIPTION
This script queries the NuGet API for all versions of a given package and deletes each version using the dotnet CLI. Requires PowerShell 7+ and the dotnet CLI to be installed. You must provide a valid NuGet API key with permission to delete the package.
.PARAMETER PackageName
The name of the NuGet package to delete.
.PARAMETER ApiKey
The NuGet API key with permission to delete the package.
.PARAMETER Source
The NuGet source/feed URL. Defaults to https://api.nuget.org/v3/index.json
.EXAMPLE
pwsh ./Remove-NugetListing.ps1 -PackageName My.Package -ApiKey <NUGET_API_KEY>
.EXAMPLE
pwsh ./Remove-NugetListing.ps1 -PackageName My.Package -ApiKey <NUGET_API_KEY> -Source "https://my.custom.nuget/feed/v3/index.json"
#>

param(
    [Parameter(Mandatory)]
    [string]$PackageName,

    [Parameter(Mandatory)]
    [string]$ApiKey,

    [string]$Source = "https://api.nuget.org/v3/index.json"
)

$ErrorActionPreference = 'Stop'

# Build the flat container API URL
$flatApiUrl = "https://api.nuget.org/v3-flatcontainer/$($PackageName.ToLowerInvariant())/index.json"

Write-Host "Querying all versions for package: $PackageName" -ForegroundColor Cyan

try {
    $response = Invoke-RestMethod -Uri $flatApiUrl
    $versions = $response.versions
} catch {
    Write-Error "Failed to query package versions. Ensure the package exists and the name is correct. $_"
    exit 1
}

if (-not $versions) {
    Write-Warning "No versions found for package $PackageName."
    exit 0
}

foreach ($version in $versions) {
    Write-Host "Deleting $PackageName $version from source $Source..." -ForegroundColor Yellow
    try {
        dotnet nuget delete $PackageName $version --source $Source --non-interactive --api-key $ApiKey --no-service-endpoint
        Write-Host "Successfully deleted $PackageName $version" -ForegroundColor Green
    } catch {
        Write-Warning "Failed to delete $PackageName $version"
    }
}

Write-Host "Completed deleting all versions of $PackageName." -ForegroundColor Cyan