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
	[string]$DotNetVersion = "10"
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
	# Add permission scopes
	$scopes = @(
		@{name="digitalinsights.admin"; adminConsentDisplayName="Admin digital insights"; adminConsentDescription="Allows the app to administrate digital insights features, templates, connectors for a signed-in user's digital insights"; userConsentDisplayName="Admin your digital insights"; userConsentDescription="Allows the app to administrate digital insights features, templates, connectors for a digital insights"},
		@{name="activity.execute"; adminConsentDisplayName="Execute feature activity triggers"; adminConsentDescription="Allows the app to execute a feature activity for digital assets"; userConsentDisplayName="Execute a feature"; userConsentDescription="Allows the app to execute a feature activity for your digital assets"},
		@{name="features.read"; adminConsentDisplayName="View feature configuration and results"; adminConsentDescription="Allows the app to view the signed-in user's feature configuration and results"; userConsentDisplayName="Read your features"; userConsentDescription="Allows the app to read your enrolled features"},
		@{name="assets.read"; adminConsentDisplayName="Read assets"; adminConsentDescription="Allows the app to read asset profiles and metadata"; userConsentDisplayName="Read your Assets"; userConsentDescription="Allows the app to read your asset profiles and metadata"},
		@{name="features.enroll"; adminConsentDisplayName="Enrolls features and write their setups"; adminConsentDescription="Allows enrollment of features, data sources and configurations."; userConsentDisplayName="Enroll any feature"; userConsentDescription="Allows app to eny any features"},
		@{name="analytics.read"; adminConsentDisplayName="Read analytics"; adminConsentDescription="Allows the app to read analytics for your digital assets"; userConsentDisplayName="Read your digital assets analytics"; userConsentDescription="Allows the app to read analytics for your digital assets"},
		@{name="assets.write"; adminConsentDisplayName="Read/Write assets"; adminConsentDescription="Allows the app to read and write to the signed in user's digital assets."; userConsentDisplayName="Read/Write your digital assets"; userConsentDescription="Allows the app to read/write your digital assets."}
	)
	foreach ($scope in $scopes) {
		az ad app permission add --id $apiAppId --api $apiAppId --scope $scope.name --admin-consent-display-name $scope.adminConsentDisplayName --admin-consent-description $scope.adminConsentDescription --user-consent-display-name $scope.userConsentDisplayName --user-consent-description $scope.userConsentDescription
	}
	# Add app roles
	$roles = @(
		@{value="AnalyticsReader"; displayName="Analytics Readers"; description="Analytic Readers can view analytics from discovery"},
		@{value="DiscoveryExecuter"; displayName="Discovery Executers"; description="Discover Executer start a discovery process"},
		@{value="FeatureManager"; displayName="Feature Managers"; description="Feature Manager can alter definitions for features and data sources."}
	)
	foreach ($role in $roles) {
		az ad app update --id $apiAppId --app-roles "[{\"allowedMemberTypes\":[\"User\"],\"description\":\"$($role.description)\",\"displayName\":\"$($role.displayName)\",\"isEnabled\":true,\"origin\":\"Application\",\"value\":\"$($role.value)\"}]"
	}
} else {
	Write-Host "API app registration $ApiAppRegistrationName already exists."
}

# Step 4: Write API EEID values to Presentation.WebApi via dotnet user-secrets
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
	$webApp = az ad app create --display-name $WebAppRegistrationName \
		--sign-in-audience AzureADMyOrg \
		--web-redirect-uris "https://localhost:7175/signin-oidc" \
		--web-logout-url "https://localhost:7175/signout-callback-oidc" \
		--web-implicit-grant true false \
		--required-resource-access '[{"resourceAppId":"$($apiApp.appId)","resourceAccess":[{"id":"402f77af-bdd3-47c5-8099-5d6c74f53749","type":"Scope"},{"id":"d64bfa41-ba81-4ef1-9169-57aa151a86bd","type":"Scope"},{"id":"086556f6-1051-4992-a29a-c91009af0d6e","type":"Scope"},{"id":"c72c07cb-c9e7-4330-8824-04579ae87e84","type":"Scope"},{"id":"ac540b65-7a26-445d-9bb6-9667d0903ad1","type":"Scope"},{"id":"f5ba6a75-8d09-4791-bfd2-ed58faf7a11d","type":"Scope"},{"id":"43d8d1d6-c30b-4528-bf25-e57a14939561","type":"Scope"}]},{"resourceAppId":"00000003-0000-0000-c000-000000000000","resourceAccess":[{"id":"64a6cdd6-aab1-4aaf-94b8-3cc8405e90d0","type":"Scope"},{"id":"14dad69e-099b-42c9-810b-d002981feec1","type":"Scope"},{"id":"e1fe6dd8-ba31-4d61-89e7-88639da4683d","type":"Scope"}]}]' \
		--query "appId" -o tsv
	$webAppId = $webApp
	Write-Host "Created Web app registration with appId: $webAppId"
	# Add app role
	az ad app update --id $webAppId --app-roles '[{"allowedMemberTypes":["User"],"description":"Admins have the ability to alter root setups that affect all tenants","displayName":"Multi-tenant Admins","isEnabled":true,"origin":"Application","value":"DigitalInsights.Admin"}]'
	# Add optional claims for idToken
	$claims = @("ctry","email","upn","ipaddr","family_name","given_name","preferred_username")
	foreach ($claim in $claims) {
		az ad app update --id $webAppId --optional-claims-id-token "[{\"name\":\"$claim\",\"essential\":false}]"
	}
	# Create client secret
	$webSecret = az ad app credential reset --id $webAppId --display-name "blazor-dev-$(Get-Date -Format yyyy)" --years 2 --query "secretText" -o tsv
	Write-Host "Created client secret for Web app registration."
	# Set permissions for Web app to use API as downstream OBO
	# (Pre-authorize Web app in API app registration)
	az ad app update --id $apiApp.appId --pre-authorized-applications "[{\"appId\":\"$webAppId\",\"permissionIds\":[\"402f77af-bdd3-47c5-8099-5d6c74f53749\",\"d64bfa41-ba81-4ef1-9169-57aa151a86bd\",\"086556f6-1051-4992-a29a-c91009af0d6e\",\"c72c07cb-c9e7-4330-8824-04579ae87e84\",\"ac540b65-7a26-445d-9bb6-9667d0903ad1\",\"f5ba6a75-8d09-4791-bfd2-ed58faf7a11d\",\"43d8d1d6-c30b-4528-bf25-e57a14939561\"]}]"
	Write-Host "Pre-authorized Web app in API app registration."
} else {
	Write-Host "Web app registration $WebAppRegistrationName already exists."
}

# Step 6: Write Web EEID values to Presentation.Blazor via dotnet user-secrets
Write-Host "Setting EntraExternalId values for Presentation.Blazor..."
Push-Location $WebProjectPath
dotnet user-secrets init
dotnet user-secrets set "EntraExternalId:Instance" $EeIdInstanceUrl
dotnet user-secrets set "EntraExternalId:TenantId" $TenantId
dotnet user-secrets set "EntraExternalId:ClientId" $webApp.appId
dotnet user-secrets set "EntraExternalId:ValidateAuthority" "true"
dotnet user-secrets set "EntraExternalId:ClientSecret" $webSecret
Pop-Location
