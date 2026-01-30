# ================================
# GitHub repo bootstrap (PowerShell)
# Creates repo, enables security, branch policy, environments
# Requires: GitHub CLI (gh) + authenticated session
# ================================
#
# Pre-requisites (auto-executed):
# - Installs GitHub CLI if not present
# - Prompts for GitHub authentication if not already authenticated
#
# Example usage (copy/paste):
#
#   .\New-GithubRepoBootstrap.ps1 -Owner goodtocode -Repo my-repo -Visibility private
#   .\New-GithubRepoBootstrap.ps1 -Owner goodtocode -Repo my-oss-repo -Oss
#
param(
  [Parameter(Mandatory = $true)][string]$Owner,
  [Parameter(Mandatory = $true)][string]$Repo,
  [ValidateSet('public', 'private')][string]$Visibility = 'private',
  [switch]$Oss # if set, will use MIT license and public visibility
)

# ---- 0) Create repository with README, .gitignore, license (if OSS)
$license = $Oss.IsPresent ? 'mit' : $null
$vis = $Oss.IsPresent ? 'public' : $Visibility

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

Write-Host "Checking if repo exists..."
$repoExists = gh repo view "$Owner/$Repo" 2>$null
if (-not $repoExists) {
  $createArgs = @(
    'repo', 'create', "$Owner/$Repo",
    "--$vis",
    '--add-readme',
    '--gitignore', 'VisualStudio'
  )
  if ($license) { $createArgs += @('--license', $license) }
  Write-Host "DEBUG: gh $($createArgs -join ' ')"
  gh @createArgs | Out-Null
  Write-Host "Created repo $Owner/$Repo"
  # Re-check repo existence after creation
  $repoExists = gh repo view "$Owner/$Repo" 2>$null
}
else {
  Write-Host "Repo $Owner/$Repo already exists. Skipping creation."
}

# ---- 1) Allow auto-merge (repo-level toggle)
# Enables future workflows to set --auto on PRs
if ($repoExists) {
  $autoMergeStatus = gh api "repos/$Owner/$Repo" | ConvertFrom-Json | Select-Object -ExpandProperty allow_auto_merge
  if (-not $autoMergeStatus) {
    gh api -X PATCH "repos/$Owner/$Repo" -f allow_auto_merge=true | Out-Null
    Write-Host "Enabled auto-merge."
  }
  else {
    Write-Host "Auto-merge already enabled."
  }
}

# ---- 2) Enable security & analysis: Secret Scanning + Push Protection (fixed payload)
if ($repoExists) {
  $repoJson = gh api "repos/$Owner/$Repo" | ConvertFrom-Json
  if ($repoJson.PSObject.Properties.Name -contains 'security_and_analysis') {
    $secStatus = $repoJson.security_and_analysis
    if ($secStatus.secret_scanning.status -ne "enabled" -or $secStatus.secret_scanning_push_protection.status -ne "enabled") {
      $ghArgs = @(
        'api',
        '-X', 'PATCH',
        "repos/$Owner/$Repo",
        '-f', 'secret_scanning.status=enabled',
        '-f', 'secret_scanning_push_protection.status=enabled'
      )
      $response = gh @ghArgs
      Write-Host "Enabled secret scanning and push protection."
    }
    else {
      Write-Host "Secret scanning and push protection already enabled."
    }
  } else {
    Write-Host "Warning: 'security_and_analysis' property not found in repo API response. Skipping secret scanning and push protection setup."
  }
}

# ---- 3) Enable Dependabot alerts & security updates (and add version updates file)
# Alerts / Security updates are repository settings endpoints.
# (If your org enforces these by default, you can skip.)
# List/enable endpoints are under Repositories API group. 
# Add dependabot.yml (version updates) if you want scheduled updates:
$dependabotYml = @"
version: 2
updates:
  - package-ecosystem: "nuget"
    directory: "/"
    schedule:
      interval: "weekly"
  - package-ecosystem: "github-actions"
    directory: "/"
    schedule:
      interval: "weekly"
"@

$tmp = New-TemporaryFile
$dependabotYml | Set-Content -NoNewline -Path $tmp
if ($repoExists) {
  $fileExists = gh api "/repos/$Owner/$Repo/contents/.github/dependabot.yml" 2>$null
  if (-not $fileExists) {
    gh api --method PUT "/repos/$Owner/$Repo/contents/.github/dependabot.yml" `
      -f message="chore: add dependabot version updates" `
      -f content="$( [Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes((Get-Content $tmp -Raw))) )" `
      -f branch="main" | Out-Null
    Write-Host "Added dependabot.yml."
  }
  else {
    Write-Host "dependabot.yml already exists. Skipping."
  }
}
Remove-Item $tmp -Force

# ---- 4) (Option A) Add Advanced CodeQL workflow file for full automation
# Or Advanced setup is workflow-based & fully automatable. [8](https://graphite.com/guides/github-merge-queue)
$codeqlYml = @"
name: CodeQL
on:
  push:
    branches: [ main, dev ]
  pull_request:
    branches: [ main, dev ]
  schedule:
    - cron: '0 6 * * 1'
permissions:
  contents: read
  security-events: write
jobs:
  analyze:
    runs-on: ubuntu-latest
    strategy:
      matrix:
        language: [ 'csharp' ]
    steps:
      - uses: actions/checkout@v4
      - uses: github/codeql-action/init@v4
        with:
          languages: '`${{ matrix.language }}'
      - uses: github/codeql-action/autobuild@v3
      - uses: github/codeql-action/analyze@v3
"@

$tmp = New-TemporaryFile
$codeqlYml | Set-Content -NoNewline -Path $tmp
if ($repoExists) {
  $fileExists = gh api "/repos/$Owner/$Repo/contents/.github/workflows/codeql-analysis.yml" 2>$null
  if (-not $fileExists) {
    gh api --method PUT "/repos/$Owner/$Repo/contents/.github/workflows/codeql-analysis.yml" `
      -f message="ci: add CodeQL advanced workflow" `
      -f content="$( [Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes((Get-Content $tmp -Raw))) )" `
      -f branch="main" | Out-Null
    Write-Host "Added CodeQL workflow."
  }
  else {
    Write-Host "CodeQL workflow already exists. Skipping."
  }
}
Remove-Item $tmp -Force

# ---- 5) Create a new ruleset called 'main-ruleset' (modern GitHub Ruleset API)
# This is the new recommended way to enforce branch policies.
if ($repoExists) {
  Write-Host "Creating 'main-ruleset' for branch 'main'..."
  $rulesetBodyObj = @{
    name        = "main-ruleset"
    target      = "branch"
    enforcement = "active"
    conditions  = @{
      ref_name = @{
        include = @("refs/heads/main")
        exclude = @()
      }
    }
    rules       = @(
        # Require PR before merging
        @{ type = "pull_request" },
        # Require linear history
        @{ type = "required_linear_history" }
    )
    # Note: To allow force-push for emergencies, add a bypass_actors array with your user/team and bypass_mode="always".
    # Example: bypass_actors = @(@{ actor_id = 123456; actor_type = "User"; bypass_mode = "always" })
  }
  $rulesetBody = $rulesetBodyObj | ConvertTo-Json -Compress -Depth 5

  $existingRulesets = gh api "/repos/$Owner/$Repo/rulesets" | ConvertFrom-Json
  $mainRuleset = $existingRulesets | Where-Object { $_.name -eq "main-ruleset" }
  if (-not $mainRuleset) {
    $response = $rulesetBody | gh api -X POST "/repos/$Owner/$Repo/rulesets" --input - -H "Accept: application/vnd.github+json"
    Write-Host "'main-ruleset' created."
  }
  else {
    Write-Host "'main-ruleset' already exists. Skipping creation."
  }
}
# Ref: https://docs.github.com/en/rest/branches/rulesets?apiVersion=2022-11-28

# ---- 6) Create Environments: development & production
# (You can set branch policy + required reviewers)
# Create/Update Environment (PUT) + optional deployment branch policy & protection rules
# Note: required_reviewers require usernames or team slugs (max 6). [7](https://docs.github.com/en/rest/deployments/environments)[12](https://docs.github.com/en/actions/reference/workflows-and-actions/deployments-and-environments)

# development
if ($repoExists) {
  Write-Host "Checking if development environment exists..."
  $devEnvResponse = gh api "repos/$Owner/$Repo/environments/development" 2>&1
  if ($devEnvResponse -match '"Not Found"' -or $devEnvResponse -match '404') {
    Write-Host "Development environment does not exist. Creating..."
    gh api -X PUT "repos/$Owner/$Repo/environments/development" `
      -H "Accept: application/vnd.github+json" | Out-Null
    Write-Host "Development environment created."
  }
  else {
    Write-Host "Development environment already exists. Skipping."
  }
}
# Optionally add a custom branch policy pattern for dev (requires extra POST endpoint under env policies).
# See community examples for adding custom branch policies after creation. [13](https://stackoverflow.com/questions/70943164/create-environment-for-repository-using-gh)

# NOTE: Replace reviewers with concrete users/teams via separate calls if needed.
if ($repoExists) {
  Write-Host "Checking if production environment exists..."
  $prodEnvResponse = gh api "repos/$Owner/$Repo/environments/production" 2>&1
  if ($prodEnvResponse -match '"Not Found"' -or $prodEnvResponse -match '404') {
    Write-Host "Production environment does not exist. Creating..."
    gh api -X PUT "repos/$Owner/$Repo/environments/production" `
      -H "Accept: application/vnd.github+json" | Out-Null
    Write-Host "Production environment created."
  }
  else {
    Write-Host "Production environment already exists. Skipping."
  }
}
# Ref: Environments API supports branch policy and protection rules. [7](https://docs.github.com/en/rest/deployments/environments)

Write-Host "✅ Bootstrap completed for $Owner/$Repo"
