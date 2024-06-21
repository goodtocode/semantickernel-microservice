param name string = 'azureblob'
param stName string

var locationShortName = toLower(replace(resourceGroup().location, ' ', ''))
var nameLower_var = toLower(replace(replace(name, '-', ''), ' ', ''))

resource nameLower 'Microsoft.Web/connections@2018-07-01-preview' = {
  name: nameLower_var
  location: locationShortName
  kind: 'V1'
  scale: null
  properties: {
    displayName: name
    customParameterValues: {}
    api: {
      name: nameLower_var
      displayName: 'Azure Storage Tables'
      description: 'Microsoft Azure Storage provides a massively scalable, durable, and highly available storage for data on the cloud, and serves as the data storage solution for modern applications. Connect to Blob Storage to perform various operations such as create, update, get and delete on blobs in your Azure Storage account.'
      iconUri: 'https://connectoricons-prod.azureedge.net/azuretables/icon_1.0.1048.1234.png'
      brandColor: '#804998'
      id: '/subscriptions/${subscription().subscriptionId}/providers/Microsoft.Web/locations/${locationShortName}/managedApis/azureblob'
      type: 'Microsoft.Web/locations/managedApis'
    }
    parameterValues: {
      accountName: stName
      accessKey: listKeys(resourceId(subscription().subscriptionId, resourceGroup().name, 'Microsoft.Storage/storageAccounts', stName), '2021-06-01').keys[0].value
    }
  }
  dependsOn: []
}