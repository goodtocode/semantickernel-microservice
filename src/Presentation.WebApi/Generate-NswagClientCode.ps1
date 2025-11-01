####################################################################################
# To execute
#   1. In powershell, set security polilcy for this script: 
#      Set-ExecutionPolicy Unrestricted -Scope Process -Force
#   2. Change directory to the script folder:
#      CD C:\Scripts (wherever your script is)
#   3. In powershell, run script: 
#      .\Generate-NswagClientCode.ps1
# Imperva Swagger: https://docs.imperva.com/bundle/cloud-application-security/page/cloud-v1-api-definition.htm
####################################################################################

param (
    [string]$SwaggerJsonPath = 'swagger',
    [string]$ApiAssembly = 'bin\Debug\net9.0\Goodtocode.SemanticKernel.Presentation.WebApi.dll',
    [string]$ApiVersion = 'v1'
)
####################################################################################
Set-ExecutionPolicy Unrestricted -Scope Process -Force
$VerbosePreference = 'SilentlyContinue' # 'Continue'
####################################################################################

$swaggerJsonPathFile = "$SwaggerJsonPath/$ApiVersion/swagger.json"
dotnet new tool-manifest --force
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:OpenAI__ApiKey = "123"
if (!(Test-Path -Path "$SwaggerJsonPath/$ApiVersion")) {
    New-Item -ItemType Directory -Path "$SwaggerJsonPath/$ApiVersion" | Out-Null
}

$swashVersion = "9.0.4"

dotnet add package Swashbuckle.AspNetCore --version $swashVersion
dotnet restore
dotnet build --configuration Debug

dotnet tool install swashbuckle.aspnetcore.cli --local --version $swashVersion
dotnet swagger tofile --output $swaggerJsonPathFile $ApiAssembly $ApiVersion

if (Test-Path -Path $swaggerJsonPathFile) {    
    Write-Host "swagger.json generated successfully with OpenAPI 3.0.0."
}
else {
    Write-Error "swagger.json was not generated. Please check for build errors or missing dependencies."
    exit 1
}

dotnet tool install nswag.consolecore --local
dotnet nswag run Generate-NswagClientCode.json