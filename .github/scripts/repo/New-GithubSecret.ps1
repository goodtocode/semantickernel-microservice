# ================================
# GitHub repo secrets (PowerShell)
# Creates secrets
# Requires: GitHub CLI (gh) + authenticated session
# ================================
#
# Pre-requisites (auto-executed):
# - Installs GitHub CLI if not present
# - Prompts for GitHub authentication if not already authenticated
#
# Example usage (copy/paste):
#
#   .\New-GithubSecret.ps1 -Owner goodtocode -Repo my-repo -Environment development -SecretName MY_SECRET -SecretValue "secret-value"
#
param(
  [Parameter(Mandatory=$true)][string]$Owner,
  [Parameter(Mandatory=$true)][string]$Repo,
  [Parameter(Mandatory=$true)][string]$Environment,
  [Parameter(Mandatory=$true)][string]$SecretName,
  [Parameter(Mandatory=$true)][string]$SecretValue
)

if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
  Write-Host "GitHub CLI not found. Installing via winget..." -ForegroundColor Red
  winget install --id GitHub.cli -e --silent
  Write-Host "GitHub CLI installed. Please restart your terminal or PowerShell session, then re-run this script." -ForegroundColor Red
  exit
}

# Check authentication
$ghAuth = gh auth status 2>$null
if (-not $ghAuth) {
  Write-Host "GitHub CLI not authenticated. Please login." -ForegroundColor Red
  gh auth login
}

gh @createArgs | Out-Null

Write-Host "Checking if repo exists..."
$repoExists = gh repo view "$Owner/$Repo" 2>$null
if (-not $repoExists) {
    Write-Host "Repo $Owner/$Repo does not exist. Cannot create secrets for a non-existent repository."
    exit
} else {
    Write-Host "Repo $Owner/$Repo already exists. Skipping creation."
}

# Check Environment
  if ($repoExists) {
  Write-Host "Checking if $Environment environment exists..."
  $envExist = gh api "repos/$Owner/$Repo/environments/$Environment" 2>$null
  if (-not $envExist) {
    Write-Host "$Environment environment does not exist. Cannot create secrets for a non-existent environment."
  }
}

# Add environment secrets (examples)
if ($repoExists -and $envExist) {
  Write-Host "Checking if $SecretName secret exists in $Environment..."
  $devSecretList = gh secret list --repo "$Owner/$Repo" --env "$Environment" 2>&1
  if ($devSecretList -match '"Not Found"' -or $devSecretList -match '404' -or -not ($devSecretList | Select-String "$SecretName")) {
    Write-Host "$SecretName secret does not exist in $Environment. Creating..."
    gh secret set "$SecretName" --body "$SecretValue" --repo "$Owner/$Repo" --env "$Environment"
    Write-Host "$SecretName secret added to $Environment."
  } else {
    Write-Host "$SecretName already exists in $Environment. Skipping."
  }
}
Write-Host "✅ Secrets completed for $Owner/$Repo in $Environment"
