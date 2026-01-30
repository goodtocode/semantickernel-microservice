
@description('The name of the Azure Function App. Must be 1-60 characters, using only alphanumeric characters and hyphens.')
@minLength(1)
@maxLength(60)
param name string

@description('The Azure region where the Function App will be deployed.')
param location string

@description('Tags to apply to the Function App resource.')
param tags object = {}

@description('The resource ID of the App Service Plan.')
@minLength(1)
param planId string

@description('The name of the Storage Account for the Function App.')
@minLength(3)
@maxLength(24)
param stName string

@description('The subscription ID for the Storage Account. Defaults to the current subscription.')
param stSubscriptionId string = subscription().subscriptionId

@description('The resource group name for the Storage Account. Defaults to the current resource group.')
param stResourceGroupName string = resourceGroup().name

@description('The Application Insights instrumentation key for the Function App.')
@minLength(1)
param appiKey string

@description('The Application Insights connection string for the Function App.')
@minLength(1)
param appiConnection string

@description('Whether to use a 32-bit worker process. Default is true.')
param use32BitWorkerProcess bool = true


@description('The environment for the Function App. Allowed values: Development, QA, Staging, Production.')
@allowed([
  'Development'
  'QA'
  'Staging'
  'Production'
])
param environmentApp string


@description('The runtime for the Function App. Allowed values: dotnet, python, dotnet-isolated. Default is dotnet.')
@allowed([
  'dotnet'
  'python'
  'dotnet-isolated'
])
param funcRuntime string = 'dotnet'


@description('The version of the Azure Functions runtime. Allowed values: 1, 2, 3, 4. Default is 4.')
@allowed([
  1
  2
  3
  4
])
param funcVersion int = 4


@description('Whether the Function App is always on. Default is false.')
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
