@description('The name of the App Service Web App. Must be 1-60 characters, using only alphanumeric characters and hyphens.')
@minLength(1)
@maxLength(60)
param name string

@description('The Azure region where the Web App will be deployed.')
param location string

@description('Tags to apply to the Web App resource.')
param tags object = {}

@description('The environment for the Web App. Allowed values: Development, QA, Staging, Production. Default is Development.')
@allowed([
  'Development'
  'QA'
  'Staging'
  'Production'
])
param environment string = 'Development'

@description('The Application Insights instrumentation key for the Web App.')
@minLength(1)
param appiKey string

@description('The Application Insights connection string for the Web App.')
@minLength(1)
param appiConnection string

@description('The resource ID of the App Service Plan.')
@minLength(1)
param planId string

@description('The kind of the Web App. Allowed values: api, app, app,linux, functionapp, functionapp,linux. Default is app.')
@allowed([
  'api'
  'app'
  'app,linux'
  'functionapp'
  'functionapp,linux'
])
param kind string = 'app'

@description('The .NET version for the Web App. Allowed values: v4.8 (for .NET Framework), 6.0, 7.0, 8.0, 9.0, 10.0 (for .NET). Default is 10.0.')
@allowed([
  'v4.8'
  '6.0'
  '7.0'
  '8.0'
  '9.0'
  '10.0' 
])
param dotnetVersion string = '10.0'

@description('Enable Always On for the App Service')
param alwaysOn bool = false

@description('Enable WebSockets for the App Service')
param websockets bool = true

resource webAppResource 'Microsoft.Web/sites@2023-12-01' = {
  name: name
  location: location
  kind: kind  
  tags: empty(tags) ? null : tags  
  properties: {    
    serverFarmId: planId
    httpsOnly: true
    siteConfig: {
      netFrameworkVersion: dotnetVersion
      ftpsState: 'Disabled'
      webSocketsEnabled: websockets
      alwaysOn: alwaysOn
      appSettings: [
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appiKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appiConnection
        }
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: environment
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
      ]
    }
  }
  identity: {
    type: 'SystemAssigned'
  }
}

output id string = webAppResource.id
