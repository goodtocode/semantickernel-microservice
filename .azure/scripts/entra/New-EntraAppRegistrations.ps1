# ============================================================================
# Script Name:   New-EntraAppRegistrations.ps1
# Description:   Creates new Entra External ID App Registrations.
# -----------------------------------------------------------------------------
# Example CLI Usage:
#   pwsh -File ./New-EntraAppRegistrations.ps1 `
#       -EntraInstanceUrl "https://your-tenant-name.ciamlogin.com" `
#       -TenantId "<your-tenant-id>" `
#       -WebAppRegistrationName "myproduct-web-dev-001" `
#       -ApiAppRegistrationName "myproduct-api-dev-001" `
#       -WebProjectPath "../../src/Presentation.Blazor" `
#       -ApiProjectPath "../../src/Presentation.WebApi"
# -----------------------------------------------------------------------------
# Notes:
#   - Requires Azure PowerShell modules (Az.Accounts, Az.Resources, etc.)
#   - Ensure you are authenticated: Connect-AzAccount
# ============================================================================
param(
	[string]$EntraInstanceUrl,
	[string]$TenantId,
	[string]$WebAppRegistrationName,
	[string]$ApiAppRegistrationName,
	[string]$WebProjectPath,
	[string]$ApiProjectPath,
	[string]$DotNetVersion = "10",
	[string]$WebRedirectUri = "https://localhost:7175/signin-oidc",
	[string]$WebLogoutUri = "https://localhost:7175/signout-callback-oidc"
)

function New-ApiRegistration {
	param(
		[string]$ApiAppRegistrationName,
		[string]$TenantId
	)
	Write-Host "Checking for API app registration: $ApiAppRegistrationName..."
	$apiApp = Get-MgApplication -Filter "displayName eq '$ApiAppRegistrationName'" -ErrorAction SilentlyContinue
	$created = $false
	if (-not $apiApp) {
		Write-Host "API app registration not found. Creating..."
		try {
			$apiApp = New-MgApplication -DisplayName $ApiAppRegistrationName -SignInAudience AzureADMyOrg
			$created = $true
		}
		catch {
			Write-Error "FATAL: Failed to create API app registration. $_.Exception.Message"
			exit 1
		}
		if (-not $apiApp -or -not $apiApp.AppId) {
			Write-Error "FATAL: API app registration was not created. Exiting script."
			exit 1
		}
		$apiAppId = $apiApp.AppId
		Write-Host "Created API app registration with appId: $apiAppId"
		$identifierUri = "api://$apiAppId"
		Update-MgApplication -ApplicationId $apiApp.Id -IdentifierUris @($identifierUri)
		Write-Host "Set IdentifierUri to $identifierUri"
		$customScopes = @(
			@{ Id = [guid]::NewGuid(); AdminConsentDisplayName = "Read assets"; AdminConsentDescription = "Allows the app to view asset data."; UserConsentDisplayName = "Read your assets"; UserConsentDescription = "Allows the app to view your assets."; IsEnabled = $true; Type = "User"; Value = "assets.read" },
			@{ Id = [guid]::NewGuid(); AdminConsentDisplayName = "Edit assets"; AdminConsentDescription = "Allows the app to create or update asset data."; UserConsentDisplayName = "Edit your assets"; UserConsentDescription = "Allows the app to create or update your assets."; IsEnabled = $true; Type = "User"; Value = "assets.write" },
			@{ Id = [guid]::NewGuid(); AdminConsentDisplayName = "Delete assets"; AdminConsentDescription = "Allows the app to delete asset data."; UserConsentDisplayName = "Delete your assets"; UserConsentDescription = "Allows the app to delete your assets."; IsEnabled = $true; Type = "User"; Value = "assets.delete" }
		)
		Update-MgApplication -ApplicationId $apiApp.Id -Api @{ OAuth2PermissionScopes = $customScopes }
		Write-Host "Added custom OAuth2 permission scopes to API app registration."
		$appRoles = @(
			@{ Id = [guid]::NewGuid(); AllowedMemberTypes = @("User"); Description = "Can view assets only."; DisplayName = "Asset Viewer"; IsEnabled = $true; Origin = "Application"; Value = "AssetViewer" },
			@{ Id = [guid]::NewGuid(); AllowedMemberTypes = @("User"); Description = "Can view and edit assets."; DisplayName = "Asset Editor"; IsEnabled = $true; Origin = "Application"; Value = "AssetEditor" },
			@{ Id = [guid]::NewGuid(); AllowedMemberTypes = @("User"); Description = "Can view, edit, and delete assets."; DisplayName = "Asset Admin"; IsEnabled = $true; Origin = "Application"; Value = "AssetAdmin" }
		)
		Update-MgApplication -ApplicationId $apiApp.Id -AppRoles $appRoles
		Write-Host "Added app roles to API app registration."
		$apiApp = Wait-ForApplicationPropagation -AppId $apiApp.AppId -ObjectId $apiApp.Id
	}
	else {
		Write-Host "API app registration $ApiAppRegistrationName already exists."
	}

	Write-Host "Adding Microsoft Graph User.Read permission to API app registration..."
	$msGraphSp = Get-MgServicePrincipal -Filter "displayName eq 'Microsoft Graph'" -ErrorAction Stop
	if (-not $msGraphSp) {
		Write-Error "FATAL: Microsoft Graph service principal not found. Exiting script."
		exit 1
	}
	$userReadPerm = $msGraphSp.Oauth2PermissionScopes | Where-Object { $_.Value -eq "User.Read" }
	if (-not $userReadPerm) {
		Write-Error "FATAL: Microsoft Graph User.Read permission not found. Exiting script."
		exit 1
	}
	$apiAppReqPerms = @{
		ResourceAppId  = $msGraphSp.AppId
		ResourceAccess = @(@{ Id = $userReadPerm.Id; Type = "Scope" })
	}
	Update-MgApplication -ApplicationId $apiApp.Id -RequiredResourceAccess @($apiAppReqPerms)
	Write-Host "Added Microsoft Graph User.Read delegated permission to API app registration."

	$apiApp = Wait-ForApplicationPropagation -AppId $apiApp.AppId
	$apiSp = Get-MgServicePrincipal -All | Where-Object { $_.AppId -eq $apiApp.AppId } | Select-Object -First 1
	if (-not $apiSp) {
		Write-Host "API service principal not found. Creating service principal for API app..."
		$apiSp = New-MgServicePrincipal -AppId $apiApp.AppId
		Start-Sleep -Seconds 5
	}
	if ($apiSp) {
		$portalConsentUrl = "https://portal.azure.com/#view/Microsoft_AAD_RegisteredApps/ApplicationMenuBlade/~/Permissions/appId/$($apiApp.AppId)/isMSAApp~/false"
		Write-Host ""
		Write-Host "ACTION REQUIRED: Grant admin consent for API app permissions in the Azure Portal:" -ForegroundColor Yellow
		Write-Host "Open the following URL in your browser:" -ForegroundColor Yellow
		Write-Host $portalConsentUrl -ForegroundColor Cyan
		Write-Host "Then click 'Grant admin consent for ...' in the API permissions blade." -ForegroundColor Yellow
		Write-Host ""
	}

	return [PSCustomObject]@{
		App        = $apiApp
		AppId      = $apiApp.AppId
		ObjectId   = $apiApp.Id
		SpObjectId = if ($apiSp) { $apiSp.Id } else { "" }
		Created    = $created
	}
}

function New-WebRegistration {
	param(
		[string]$WebAppRegistrationName,
		[string]$TenantId,
		[string]$WebRedirectUri,
		[string]$WebLogoutUri
	)
	Write-Host "Checking for Web app registration: $WebAppRegistrationName..."
	$webApp = Get-MgApplication -Filter "displayName eq '$WebAppRegistrationName'" -ErrorAction SilentlyContinue
	$created = $false
	$webSecret = $null
	if (-not $webApp) {
		Write-Host "Web app registration not found. Creating..."
		try {
			$webApp = New-MgApplication -DisplayName $WebAppRegistrationName -SignInAudience AzureADMyOrg -Web @{ RedirectUris = @($WebRedirectUri); LogoutUrl = $WebLogoutUri }
			$created = $true
		}
		catch {
			Write-Error "FATAL: Failed to create Web app registration. $_.Exception.Message"
			exit 1
		}
		if (-not $webApp -or -not $webApp.AppId) {
			Write-Error "FATAL: Web app registration was not created. Exiting script."
			exit 1
		}
		Write-Host "Created Web app registration with appId: $($webApp.AppId)"

		Update-MgApplication -ApplicationId $webApp.Id -Web @{ ImplicitGrantSettings = @{ EnableIdTokenIssuance = $false; EnableAccessTokenIssuance = $true } }
		Write-Host "Enabled access token issuance for implicit/hybrid flows in Web app registration."

		$webAppRoles = @(
			@{
				Id                 = [Guid]::Parse("4c335757-d75a-4d79-8efc-ddcded89450b")
				AllowedMemberTypes = @("User")
				Description        = "Admins have the ability to alter root setups that affect all tenants"
				DisplayName        = "Multi-tenant Admins"
				IsEnabled          = $true
				Origin             = "Application"
				Value              = "Asset.Admin"
			}
		)
		Update-MgApplication -ApplicationId $webApp.Id -AppRoles $webAppRoles
		Write-Host "Added Asset.Admin app role to Web app registration."

		try {
			$passwordCredential = @{ EndDateTime = (Get-Date).AddYears(2) }
			$webSecretObj = Add-MgApplicationPassword -ApplicationId $webApp.Id -PasswordCredential $passwordCredential
		}
		catch {
			Write-Error "FATAL: Failed to create client secret for Web app registration. $_.Exception.Message"
			exit 1
		}
		if (-not $webSecretObj -or -not $webSecretObj.SecretText) {
			Write-Error "FATAL: Web app client secret was not created. Exiting script."
			exit 1
		}
		$webSecret = $webSecretObj.SecretText
		Write-Host "Created client secret for Web app registration."
		$webApp = Wait-ForApplicationPropagation -AppId $webApp.AppId -ObjectId $webApp.Id
	}
	else {
		Write-Host "Web app registration $WebAppRegistrationName already exists."
	}
	Write-Host "Adding Microsoft Graph User.Read, email, profile delegated permissions to Web app registration..."
	$msGraphSp = Get-MgServicePrincipal -Filter "displayName eq 'Microsoft Graph'" -ErrorAction Stop
	if (-not $msGraphSp) {
		Write-Error "FATAL: Microsoft Graph service principal not found. Exiting script."
		exit 1
	}
	$delegatedPerms = @("User.Read", "email", "profile")
	$permScopes = @()
	foreach ($perm in $delegatedPerms) {
		$scope = $msGraphSp.Oauth2PermissionScopes | Where-Object { $_.Value -eq $perm }
		if (-not $scope) {
			Write-Error "FATAL: Microsoft Graph permission $perm not found. Exiting script."
			exit 1
		}
		$permScopes += @{ Id = $scope.Id; Type = "Scope" }
	}
	$webAppReqPerms = @()
	$webAppReqPerms += @{
		ResourceAppId  = $msGraphSp.AppId
		ResourceAccess = $permScopes
	}
	$apiApp = Get-MgApplication -Filter "displayName eq '$ApiAppRegistrationName'" -ErrorAction Stop
	if (-not $apiApp) {
		Write-Error "FATAL: API app registration $ApiAppRegistrationName not found. Exiting script."
		exit 1
	}
	$customScopes = @("assets.read", "assets.write", "assets.delete")
	$apiScopes = @()
	foreach ($scope in $customScopes) {
		$apiScope = $apiApp.Api.Oauth2PermissionScopes | Where-Object { $_.Value -eq $scope }
		if (-not $apiScope) {
			Write-Error "FATAL: API scope $scope not found in API app registration. Exiting script."
			exit 1
		}
		$apiScopes += @{ Id = $apiScope.Id; Type = "Scope" }
	}
	$webAppReqPerms += @{
		ResourceAppId  = $apiApp.AppId
		ResourceAccess = $apiScopes
	}
	Update-MgApplication -ApplicationId $webApp.Id -RequiredResourceAccess $webAppReqPerms
	Write-Host "Added Microsoft Graph User.Read, email, profile delegated permissions to Web app registration."

	$webApp = Wait-ForApplicationPropagation -AppId $webApp.AppId -ObjectId $webApp.Id
	$webSp = Get-MgServicePrincipal -All | Where-Object { $_.AppId -eq $webApp.AppId } | Select-Object -First 1
	if (-not $webSp) {
		Write-Host "Web service principal not found. Creating service principal for Web app..."
		$webSp = New-MgServicePrincipal -AppId $webApp.AppId
		Start-Sleep -Seconds 5
	}
	if ($webSp) {
		$portalConsentUrl = "https://portal.azure.com/#view/Microsoft_AAD_RegisteredApps/ApplicationMenuBlade/~/Permissions/appId/$($webApp.AppId)/isMSAApp~/false"
		Write-Host ""
		Write-Host "ACTION REQUIRED: Grant admin consent for Web app permissions in the Azure Portal:" -ForegroundColor Yellow
		Write-Host "Open the following URL in your browser:" -ForegroundColor Yellow
		Write-Host $portalConsentUrl -ForegroundColor Cyan
		Write-Host "Then click 'Grant admin consent for ...' in the API permissions blade." -ForegroundColor Yellow
		Write-Host ""
	}

	$optionalClaims = @{
		idToken = @(
			@{ name = "ctry" },
			@{ name = "email" },
			@{ name = "family_name" },
			@{ name = "given_name" },
			@{ name = "ipaddr" },
			@{ name = "preferred_username" },
			@{ name = "upn" }
		)
	}
	Update-MgApplication -ApplicationId $webApp.Id -OptionalClaims $optionalClaims
	Write-Host "Added optional claims (ctry, email, family_name, given_name, ipaddr, preferred_username, upn) to Web app registration."

	$webApp = Wait-ForApplicationPropagation -AppId $webApp.AppId
	$webSp = Get-MgServicePrincipal -All | Where-Object { $_.AppId -eq $webApp.AppId } | Select-Object -First 1
	return [PSCustomObject]@{
		App        = $webApp
		AppId      = $webApp.AppId
		ObjectId   = $webApp.Id
		SpObjectId = if ($webSp) { $webSp.Id } else { "" }
		Created    = $created
		Secret     = $webSecret
	}
}

function New-Auth {
	param(
		[string]$TenantId
	)
	function New-AzLogin {
		param([string]$TenantId)
		Write-Host "Checking Azure authentication..."
		$azContext = Get-AzContext -ErrorAction SilentlyContinue
		if ($azContext -and $azContext.Tenant.Id -eq $TenantId) {
			Write-Host "Already authenticated to Azure tenant $TenantId."
		}
		else {
			Write-Host "Logging into Azure..."
			Connect-AzAccount -Tenant $TenantId | Out-Null
			Write-Host "Logged in to Azure tenant $TenantId."
		}
	}
	function New-MgLogin {
		param([string]$TenantId)
		$mgContext = $null
		try {
			$mgContext = Get-MgContext -ErrorAction Stop
		}
		catch {}
		if ($mgContext -and $mgContext.TenantId -eq $TenantId -and $mgContext.Account) {
			Write-Host "Already authenticated to Microsoft Graph for tenant $TenantId."
		}
		else {
			try {
				Write-Host "Connecting to Microsoft Graph..."
				Connect-MgGraph -TenantId $TenantId -Scopes "Application.ReadWrite.All", "Directory.ReadWrite.All" | Out-Null
				$mgContext = Get-MgContext -ErrorAction Stop
				Write-Host "Connected to Microsoft Graph for tenant $TenantId."
			}
			catch {
				Write-Error "FATAL: Microsoft Graph authentication failed. Exiting script."
				exit 1
			}
		}
	}
	New-AzLogin -TenantId $TenantId
	New-MgLogin -TenantId $TenantId
}

function Install-Prerequisites {
	param(
		[string]$DotNetVersion
	)
	Write-Host "Checking prerequisites..."
	$dotnetInstalled = & dotnet --list-sdks | Select-String "^$DotNetVersion\."
	if (-not $dotnetInstalled) {
		Write-Host ".NET SDK $DotNetVersion not found. Installing via winget..."
		winget install --id Microsoft.DotNet.SDK.$DotNetVersion -e --silent
	}
	else {
		Write-Host ".NET SDK $DotNetVersion is already installed."
	}
	$modules = @("Az.Accounts", "Az.Resources", "Microsoft.Graph.Applications")
	foreach ($module in $modules) {
		if (-not (Get-Module -ListAvailable -Name $module)) {
			Write-Host "Installing PowerShell module: $module"
			Install-Module $module -Scope CurrentUser -Force
		}
		else {
			Write-Host "PowerShell module $module is already installed."
		}
	}
	# Import modules here so they're available everywhere
	Import-Module Az.Accounts -ErrorAction Stop
	Import-Module Az.Resources -ErrorAction Stop
	Import-Module Microsoft.Graph.Applications -ErrorAction Stop
}

function Set-ProjectUserSecrets {
	param(
		[string]$ProjectPath,
		[hashtable]$Secrets
	)
	if (Test-Path $ProjectPath) {
		Write-Host "Setting EntraExternalId values for $ProjectPath"
		Push-Location $ProjectPath
		dotnet user-secrets init
		foreach ($key in $Secrets.Keys) {
			dotnet user-secrets set $key $Secrets[$key]
		}
		Pop-Location
	}
	else {
		Write-Warning "*** CRITICAL: Project path '$ProjectPath' not found. Skipping dotnet user-secrets. App registrations will continue, but user-secrets are NOT set. ***"
	}
}

function Wait-ForApplicationPropagation {
	param(
		[string]$AppId,
		[string]$ObjectId = $null,
		[int]$MaxRetries = 10,
		[int]$DelaySeconds = 2
	)
	$retryCount = 0
	$app = $null
	do {
		Start-Sleep -Seconds $DelaySeconds
		if ($ObjectId) {
			$app = Get-MgApplication -ApplicationId $ObjectId -ErrorAction SilentlyContinue
		}
		if (-not $app -and $AppId) {
			$app = Get-MgApplication -Filter "appId eq '$AppId'" -ErrorAction SilentlyContinue
		}
		$retryCount++
	} while (-not $app -and $retryCount -lt $MaxRetries)
	if (-not $app) {
		Write-Error "FATAL: Application $AppId was not found after creation and waiting. Exiting script."
		exit 1
	}
	return $app
}

function Write-OutputSummary {
	param(
		[string]$TenantId,
		[string]$EntraInstanceUrl,
		[object]$ApiApp,
		[object]$WebApp,
		[string]$WebRedirectUri,
		[string]$WebLogoutUri
	)
	Write-Host "`n================= OUTPUT SUMMARY ================="
	Write-Host "TenantId: $TenantId"
	Write-Host "Instance: $EntraInstanceUrl"
	Write-Host "API AppId: $($ApiApp.AppId)"
	Write-Host "API ObjectId: $($ApiApp.ObjectId)"
	Write-Host "API Service Principal ObjectId: $($ApiApp.SpObjectId)"
	Write-Host "Web AppId: $($WebApp.AppId)"
	Write-Host "Web ObjectId: $($WebApp.ObjectId)"
	Write-Host "Web Service Principal ObjectId: $($WebApp.SpObjectId)"
	Write-Host "Web Redirect URI: $WebRedirectUri"
	Write-Host "Web Logout URI: $WebLogoutUri"
	Write-Host "=================================================`n"
}

# Step 1: Install prerequisites
Install-Prerequisites -DotNetVersion $DotNetVersion

# Step 2: Authenticate to Azure and Microsoft Graph
New-Auth -TenantId $TenantId

# Step 3: Create or get API app registration using function
$apiReg = New-ApiRegistration -ApiAppRegistrationName $ApiAppRegistrationName -TenantId $TenantId
$apiApp = $apiReg

# Step 4: Write API EEID values to $ApiProjectPath via dotnet user-secrets
$apiSecrets = @{
	"EntraExternalId:Instance"          = $EntraInstanceUrl
	"EntraExternalId:TenantId"          = $TenantId
	"EntraExternalId:ClientId"          = $apiApp.AppId
	"EntraExternalId:ValidateAuthority" = "true"
}
Set-ProjectUserSecrets -ProjectPath $ApiProjectPath -Secrets $apiSecrets

# Step 5: Create or get Web app registration using function and capture output
$webReg = New-WebRegistration -WebAppRegistrationName $WebAppRegistrationName -TenantId $TenantId -WebRedirectUri $WebRedirectUri -WebLogoutUri $WebLogoutUri
$webApp = $webReg

# Step 6: Write Web EEID values to $WebProjectPath via dotnet user-secrets
$webSecrets = @{
	"BackEndApi:ClientId"          		= $apiApp.AppId
	"EntraExternalId:Instance"          = $EntraInstanceUrl
	"EntraExternalId:TenantId"          = $TenantId
	"EntraExternalId:ClientId"          = $webApp.AppId
	"EntraExternalId:ValidateAuthority" = "true"
	"EntraExternalId:ClientSecret"      = $webApp.Secret
}
Set-ProjectUserSecrets -ProjectPath $WebProjectPath -Secrets $webSecrets

# Step 7: Output summary of created app registrations
Write-OutputSummary -TenantId $TenantId -EntraInstanceUrl $EntraInstanceUrl -ApiApp $apiApp -WebApp $webApp -WebRedirectUri $WebRedirectUri -WebLogoutUri $WebLogoutUri