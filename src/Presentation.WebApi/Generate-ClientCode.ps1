# Generate-ClientCode.ps1

# Ensure NSwag.ConsoleCore is installed
dotnet tool install -g Nswag.ConsoleCore

# Generate client code from the Swagger document
nswag openapi2csclient `
    /input:swagger/v1/swagger.json `
    /output:../Presentation.Blazor.Client/WebApiClient/GeneratedClient.cs `
    /namespace:Goodtocode.Presentation.WebApi.Client
