param name string
param location string
param tags object = {}
param planId string
param stName string
param stSubscriptionId string = subscription().subscriptionId
param stResourceGroupName string = resourceGroup().name
param appiKey string
param appiConnection string
param use32BitWorkerProcess bool = true

@allowed([
  'Development'
  'QA'
  'Staging'
  'Production'
])
param environmentApp string

@allowed([
  'dotnet'
  'python'
  'dotnet-isolated'
])
param funcRuntime string = 'dotnet'

@allowed([
  1
  2
  3
  4
])
param funcVersion int = 4

param alwaysOn bool = false

resource functionapp 'Microsoft.Web/sites@2023-12-01' = {
  name: name 
  kind: 'functionapp'
  location: location
  tags: empty(tags) ? null : tags
  properties: {
    serverFarmId: planId
    siteConfig: {
      alwaysOn: alwaysOn
      appSettings: [
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~${funcVersion}'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: funcRuntime
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appiKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appiConnection
        }
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${stName};AccountKey=${listKeys(resourceId(stSubscriptionId, stResourceGroupName, 'Microsoft.Storage/storageAccounts', stName), '2019-06-01').keys[0].value};EndpointSuffix=core.windows.net'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${stName};AccountKey=${listKeys(resourceId(stSubscriptionId, stResourceGroupName, 'Microsoft.Storage/storageAccounts', stName), '2019-06-01').keys[0].value};EndpointSuffix=core.windows.net'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: toLower(name)
        }
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: environmentApp
        }
        {
          name: 'AZURE_FUNCTIONS_ENVIRONMENT'
          value: environmentApp
        }
      ]
      use32BitWorkerProcess: use32BitWorkerProcess
    }    
  }
  identity: {
    type: 'SystemAssigned'
  }
}
