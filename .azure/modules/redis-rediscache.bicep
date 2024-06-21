@description('Specify the name of the Azure Redis Cache to create.')
param name string = 'redis-${uniqueString(resourceGroup().id)}'

@description('Location of all resources')
param location string = toLower(replace(resourceGroup().location, ' ', ''))

@description('Specify the pricing tier of the new Azure Redis Cache.')
@allowed([
  'Basic'
  'Standard'
  'Premium'
])
param sku string = 'Basic'

@description('Specify the family for the sku. C = Basic/Standard, P = Premium.')
@allowed([
  'C'
  'P'
])
param family string = 'C'

@description('Specify the size of the new Azure Redis Cache instance. Valid values: for C (Basic/Standard) family (0, 1, 2, 3, 4, 5, 6), for P (Premium) family (1, 2, 3, 4)')
@allowed([
  0
  1
  2
  3
  4
  5
  6
])
param capacity int = 1

@description('Specify a boolean value that indicates whether to allow access via non-SSL ports.')
param enableNonSslPort bool = false

@description('Specify a boolean value that indicates whether diagnostics should be saved to the specified storage account.')
param enableDiagnostics bool = false

@description('Specify the name of an existing storage account for diagnostics.')
param stName string

@description('Specify the resource group name of an existing storage account for diagnostics.')
param storageResourceGroup string = resourceGroup().name

resource name_resource 'Microsoft.Cache/redis@2020-06-01' = {
  name: name
  location: location
  properties: {
    enableNonSslPort: enableNonSslPort
    minimumTlsVersion: '1.2'
    sku: {
      capacity: capacity
      family: family
      name: sku
    }
  }
}

resource microsoft_insights_diagnosticSettings_name 'microsoft.insights/diagnosticSettings@2017-05-01-preview' = {
  scope: name_resource
  name: name
  properties: {
    storageAccountId: extensionResourceId('/subscriptions/${subscription().subscriptionId}/resourceGroups/${storageResourceGroup}', 'Microsoft.Storage/storageAccounts', stName)
    metrics: [
      {
        timeGrain: 'AllMetrics'
        enabled: enableDiagnostics
        retentionPolicy: {
          days: 90
          enabled: enableDiagnostics
        }
      }
    ]
  }
}