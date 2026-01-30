
@description('The name of the Blob Storage API Connection. Must be 1-80 characters, using only alphanumeric characters and hyphens.')
@minLength(1)
@maxLength(80)
param name string

@description('The name of the target Storage Account for the connection.')
@minLength(3)
@maxLength(24)
param stName string

var locationShortName = toLower(replace(resourceGroup().location, ' ', ''))
var nameLower = toLower(replace(replace(name, '-', ''), ' ', ''))
var storageId = resourceId('Microsoft.Storage/storageAccounts', stName)

resource connection 'Microsoft.Web/connections@2016-06-01' = {
  name: nameLower
  location: locationShortName
    properties: {
    displayName: name
    customParameterValues: {}
    api: {
      name: '${nameLower}azureblob'
      displayName: 'Azure Blob Storage'
      description: 'Microsoft Azure Storage provides a massively scalable, durable, and highly available storage for data on the cloud, and serves as the data storage solution for modern applications. Connect to Blob Storage to perform various operations such as create, update, get and delete on blobs in your Azure Storage account.'
      iconUri: 'https://connectoricons-prod.azureedge.net/releases/v1.0.1507/1.0.1507.2528/azureblob/icon.png'
      brandColor: '#804998'
      id: '/subscriptions/${subscription().subscriptionId}/providers/Microsoft.Web/locations/${locationShortName}/managedApis/azureblob'
      type: 'Microsoft.Web/locations/managedApis'
    }
    parameterValues: {
      accountName: stName
      accessKey: listKeys(storageId, '2019-04-01').keys[0].value
    }
  }
  dependsOn: []
}
