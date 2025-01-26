####################################################################################
# To execute
#   1. In powershell, set security polilcy for this script: 
#      Set-ExecutionPolicy Unrestricted -Scope Process -Force
#   2. Change directory to the script folder:
#      CD C:\Scripts (wherever your script is)
#   3. In powershell, run script: 
#      .\Update-LoadBalancer.ps1 -IPAddress 111.222.333.4444 -ServerId 12345 -ApiKey 00000000-0000-0000-0000-000000000000 -ApiId 12345
# Imperva Swagger: https://docs.imperva.com/bundle/cloud-application-security/page/cloud-v1-api-definition.htm
####################################################################################

param (
 	[string]$SwaggerJsonPath = 'swagger',
    [string]$ApiAssembly = 'bin\Debug\net9.0\Goodtocode.SemanticKernel.Presentation.WebApi.dll',
	[string]$ApiVersion = 'v1',
	[string]$ClientPathFile = '../Presentation.Blazor.Rcl/Clients/WebApiClient.cs',
	[string]$ClientNamespace = 'Goodtocode.Presentation.WebApi.Client'
)
####################################################################################
Set-ExecutionPolicy Unrestricted -Scope Process -Force
$VerbosePreference = 'SilentlyContinue' # 'Continue'
####################################################################################

$swaggerJsonPathFile = "$SwaggerJsonPath/$ApiVersion/swagger.json"

# Setup tools
dotnet new tool-manifest --force

# Set environment vars necessary for WebApi to run
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:OpenAI__ApiKey = "123"

# Generate swagger.json
dotnet tool install swashbuckle.aspnetcore.cli
dotnet swagger tofile --output $swaggerJsonPathFile $ApiAssembly $ApiVersion

# Generate class
dotnet tool install Nswag.ConsoleCore
#nswag openapi2csclient /input:$swaggerJsonPathFile /output:$ClientPathFile /namespace:$ClientNamespace # No longer supported? /generateCodeSettings.jsonSerializerSettings:systemtextjson
nswag run Generate-NswagClientCode.json