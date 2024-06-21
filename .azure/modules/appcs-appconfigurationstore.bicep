@description('Specifies the name of the App Configuration store.')
param name string

@description('Specifies the sku of the App Configuration store.')
@allowed([
  'free'
  'standard'
])
param sku string = 'free'

@description('Specifies the Azure location where the app configuration store should be created.')
param location string = toLower(replace(resourceGroup().location, ' ', ''))

@description('Sentinel key to refresh configs for no label and Development label.')
param appcsKeys array = [
  'Shared:Sentinel'
  'Shared:Sentinel$Development'
]

@description('Sentinel value is 1. When changed to 0, will trigger config refreshes.')
param appcsValues array = [
  '1'
  '1'
]

@description('Specifies the content type of the key-value resources. For feature flag, the value should be application/vnd.microsoft.appconfig.ff+json;charset=utf-8. For Key Value reference, the value should be application/vnd.microsoft.appconfig.keyvaultref+json;charset=utf-8. Otherwise, it\'s optional.')
param contentType string = 'text/plain'

@description('Adds tags for the key-value resources. It\'s optional')
param tags object = {
  tag1: 'tag-value-1'
  tag2: 'tag-value-2'
}

resource name_resource 'Microsoft.AppConfiguration/configurationStores@2020-07-01-preview' = {
  name: name
  location: location
  sku: {
    name: sku
  }
}

resource name_appcsKeys 'Microsoft.AppConfiguration/configurationStores/keyValues@2020-07-01-preview' = [for (item, i) in appcsKeys: {
  name: '${name}/${item}'
  properties: {
    value: appcsValues[i]
    contentType: contentType
    tags: tags
  }
  dependsOn: [
    name_resource
  ]
}]

output reference_key_value_value string = reference(resourceId('Microsoft.AppConfiguration/configurationStores/keyValues', name, appcsKeys[0]), '2020-07-01-preview').value
output reference_key_value_object object = reference(resourceId('Microsoft.AppConfiguration/configurationStores/keyValues', name, appcsKeys[1]), '2020-07-01-preview')