@minLength(1)
param planName string

@minLength(1)
param name string
param appiKey string
param appiConnection string

@minLength(1)
@allowed([
  'Development'
  'QA'
  'Staging'
  'Production'
])
param rgEnvironment string = 'Development'

var webSiteName_var = name

resource webSiteName 'Microsoft.Web/sites@2022-03-01' = {
  name: webSiteName_var
  kind: 'app'
  location: resourceGroup().location
  tags: {
    'hidden-related:${resourceGroup().id}/providers/Microsoft.Web/serverfarms/${planName}': 'Resource'
    displayName: 'Website'
  }
  properties: {
    name: webSiteName_var
    serverFarmId: resourceId('Microsoft.Web/serverfarms', planName)
    siteConfig: {
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
          value: rgEnvironment
        }
      ]
    }
  }
}
