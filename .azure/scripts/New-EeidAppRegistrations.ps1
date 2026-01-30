# EEID App Registration and Secret Setup Script
# Location: .azure/scripts/New-EeidAppRegistrations.ps1
# Description: Idempotently installs prerequisites, creates/updates EEID app registrations, sets permissions, and configures dotnet user-secrets for Presentation.WebApi and Presentation.Blazor.
param(
	[string]$EeIdInstanceUrl,
	[string]$TenantId,
	[string]$SubscriptionId,
	[string]$WebAppRegistrationName = "web-semker-dev",
	[string]$WebProjectPath = "../../src/Presentation.Blazor",
	[string]$ApiAppRegistrationName = "api-semker-dev",
	[string]$ApiProjectPath = "../../src/Presentation.WebApi",
	[string]$DotNetVersion = "10",
	[string]$WebRedirectUri = "https://localhost:7175/signin-oidc",
	[string]$WebLogoutUri = "https://localhost:7175/signout-callback-oidc"
)

# Step 1: Install prerequisites (az cli, dotnet sdk, modules)
Write-Host "Checking prerequisites..."

# Check and install Azure CLI
if (-not (Get-Command az -ErrorAction SilentlyContinue)) {
	Write-Host "Azure CLI not found. Installing via winget..."
	winget install --id Microsoft.AzureCLI -e --silent
} else {
	Write-Host "Azure CLI is already installed."
}

# Check and install .NET SDK
$dotnetInstalled = & dotnet --list-sdks | Select-String "^$DotNetVersion\."
if (-not $dotnetInstalled) {
	Write-Host ".NET SDK $DotNetVersion not found. Installing via winget..."
	winget install --id Microsoft.DotNet.SDK.$DotNetVersion -e --silent
} else {
	Write-Host ".NET SDK $DotNetVersion is already installed."
}

# Check and install PowerShell modules
$modules = @("Az.Accounts", "Az.Resources")
foreach ($module in $modules) {
	if (-not (Get-Module -ListAvailable -Name $module)) {
		Write-Host "Installing PowerShell module: $module"
		Install-Module $module -Scope CurrentUser -Force
	} else {
		Write-Host "PowerShell module $module is already installed."
	}
}

# Step 2: Login to Azure and set EEID tenant/subscription
Write-Host "Logging into Azure..."
$azLoggedIn = az account show 2>$null
if (-not $azLoggedIn) {
	az login --tenant $TenantId
	Write-Host "Logged in to Azure tenant $TenantId."
} else {
	Write-Host "Already logged in to Azure."
}

Write-Host "Setting Azure subscription..."
$currentSub = az account show --query id -o tsv
if ($currentSub -ne $SubscriptionId) {
	az account set --subscription $SubscriptionId
	Write-Host "Azure subscription set to $SubscriptionId."
} else {
	Write-Host "Azure subscription already set to $SubscriptionId."
}

# Step 3: Check for API app registration by name; create if missing
Write-Host "Checking for API app registration: $ApiAppRegistrationName..."
$apiApp = az ad app list --display-name $ApiAppRegistrationName --query "[0]" -o json | ConvertFrom-Json
if (-not $apiApp) {
	Write-Host "API app registration not found. Creating..."
	$apiApp = az ad app create `
		--display-name $ApiAppRegistrationName `
		--sign-in-audience AzureADMyOrg `
		--identifier-uris "api://$ApiAppRegistrationName" `
		--required-resource-access '[{"resourceAppId":"00000003-0000-0000-c000-000000000000","resourceAccess":[{"id":"e1fe6dd8-ba31-4d61-89e7-88639da4683d","type":"Scope"}]}]' `
		--api-access-token-version 2 `
		--query "appId" -o tsv
	$apiAppId = $apiApp
	Write-Host "Created API app registration with appId: $apiAppId"
	# Add permission scopes and capture their IDs
	$scopeIdMap = @{}
	$scopes = @(
		@{name="assets.read"; adminConsentDisplayName="Read assets"; adminConsentDescription="Allows the app to view asset data."; userConsentDisplayName="Read your assets"; userConsentDescription="Allows the app to view your assets."},
		@{name="assets.write"; adminConsentDisplayName="Edit assets"; adminConsentDescription="Allows the app to create or update asset data."; userConsentDisplayName="Edit your assets"; userConsentDescription="Allows the app to create or update your assets."},
		@{name="assets.delete"; adminConsentDisplayName="Delete assets"; adminConsentDescription="Allows the app to delete asset data."; userConsentDisplayName="Delete your assets"; userConsentDescription="Allows the app to delete your assets."}
	)
	foreach ($scope in $scopes) {
		$scopeResult = az ad app permission add --id $apiAppId --api $apiAppId --scope $scope.name --admin-consent-display-name $scope.adminConsentDisplayName --admin-consent-description $scope.adminConsentDescription --user-consent-display-name $scope.userConsentDisplayName --user-consent-description $scope.userConsentDescription --query "id" -o tsv
		$scopeIdMap[$scope.name] = $scopeResult
	}
	# Add app roles
	$roles = @(
		@{value="AssetViewer"; displayName="Asset Viewer"; description="Can view assets only."},
		@{value="AssetEditor"; displayName="Asset Editor"; description="Can view and edit assets."},
		@{value="AssetAdmin"; displayName="Asset Admin"; description="Can view, edit, and delete assets."}
	)
	foreach ($role in $roles) {
		az ad app update --id $apiAppId --app-roles "[{\"allowedMemberTypes\":[\"User\"],\"description\":\"$($role.description)\",\"displayName\":\"$($role.displayName)\",\"isEnabled\":true,\"origin\":\"Application\",\"value\":\"$($role.value)\"}]"
	}
	# Query the app registration to get all scope IDs
	$apiAppObj = az ad app show --id $apiAppId | ConvertFrom-Json
	$apiScopes = @{}
	foreach ($scope in $apiAppObj.api.oauth2PermissionScopes) {
		$apiScopes[$scope.value] = $scope.id
	}
} else {
	Write-Host "API app registration $ApiAppRegistrationName already exists."
	$apiAppId = $apiApp.appId
	$apiAppObj = az ad app show --id $apiAppId | ConvertFrom-Json
	$apiScopes = @{}
	foreach ($scope in $apiAppObj.api.oauth2PermissionScopes) {
		$apiScopes[$scope.value] = $scope.id
	}
}

# Step 4: Write API EEID values to $ApiProjectPath via dotnet user-secrets
Write-Host "Setting EntraExternalId values for $ApiProjectPath"
Push-Location $ApiProjectPath
dotnet user-secrets init
dotnet user-secrets set "EntraExternalId:Instance" $EeIdInstanceUrl
dotnet user-secrets set "EntraExternalId:TenantId" $TenantId
dotnet user-secrets set "EntraExternalId:ClientId" $apiApp.appId
dotnet user-secrets set "EntraExternalId:ValidateAuthority" "true"
Pop-Location

# Step 5: Check for Web app registration by name; create if missing
Write-Host "Checking for Web app registration: $WebAppRegistrationName..."
$webApp = az ad app list --display-name $WebAppRegistrationName --query "[0]" -o json | ConvertFrom-Json
if (-not $webApp) {
	Write-Host "Web app registration not found. Creating..."
	$apiResourceAccess = @()
	foreach ($scopeName in $apiScopes.Keys) {
		$apiResourceAccess += @{ id = $apiScopes[$scopeName]; type = "Scope" }
	}
	$requiredResourceAccess = @(
		@{ resourceAppId = $apiAppId; resourceAccess = $apiResourceAccess },
		@{ resourceAppId = "00000003-0000-0000-c000-000000000000"; resourceAccess = @(
			@{ id = "64a6cdd6-aab1-4aaf-94b8-3cc8405e90d0"; type = "Scope" },
			@{ id = "14dad69e-099b-42c9-810b-d002981feec1"; type = "Scope" },
			@{ id = "e1fe6dd8-ba31-4d61-89e7-88639da4683d"; type = "Scope" }
		) }
	) | ConvertTo-Json -Compress
	$webApp = az ad app create --display-name $WebAppRegistrationName `
		--sign-in-audience AzureADMyOrg `
		--web-redirect-uris $WebRedirectUri `
		--web-logout-url $WebLogoutUri `
		--web-implicit-grant true false `
		--required-resource-access $requiredResourceAccess `
		--query "appId" -o tsv
	$webAppId = $webApp
	Write-Host "Created Web app registration with appId: $webAppId"
	# Add app role
	az ad app update --id $webAppId --app-roles '[{"allowedMemberTypes":["User"],"description":"Admins have the ability to alter root setups that affect all tenants","displayName":"Multi-tenant Admins","isEnabled":true,"origin":"Application","value":"admin"}]'
	# Add optional claims for idToken
	$claims = @("ctry","email","upn","ipaddr","family_name","given_name","preferred_username")
	foreach ($claim in $claims) {
		az ad app update --id $webAppId --optional-claims-id-token "[{\"name\":\"$claim\",\"essential\":false}]"
	}
	# Create client secret
	$webSecret = az ad app credential reset --id $webAppId --display-name "$WebAppRegistrationName-$(Get-Date -Format yyyy)" --years 2 --query "secretText" -o tsv
	Write-Host "Created client secret for Web app registration."
	# Set permissions for Web app to use API as downstream OBO
	# (Pre-authorize Web app in API app registration)
	$permissionIds = @()
	foreach ($scopeName in $apiScopes.Keys) {
		$permissionIds += $apiScopes[$scopeName]
	}
	$preAuthApps = @(@{ appId = $webAppId; permissionIds = $permissionIds }) | ConvertTo-Json -Compress
	az ad app update --id $apiAppId --pre-authorized-applications $preAuthApps
	Write-Host "Pre-authorized Web app in API app registration."
} else {
	Write-Host "Web app registration $WebAppRegistrationName already exists."
}

# Step 6: Write Web EEID values to $WebProjectPath via dotnet user-secrets
Write-Host "Setting EntraExternalId values for $WebProjectPath"
Push-Location $WebProjectPath
dotnet user-secrets init
dotnet user-secrets set "EntraExternalId:Instance" $EeIdInstanceUrl
dotnet user-secrets set "EntraExternalId:TenantId" $TenantId
dotnet user-secrets set "EntraExternalId:ClientId" $webApp.appId
dotnet user-secrets set "EntraExternalId:ValidateAuthority" "true"
dotnet user-secrets set "EntraExternalId:ClientSecret" $webSecret
Pop-Location
