@description('That name is the name of our application. It has to be unique.Type a name followed by your resource group name. (<name>-<resourceGroupName>)')
param name string = 'CognitiveService-${uniqueString(resourceGroup().id)}'

@description('Sku (pricing tier) of this resource')
@allowed([
  'F0'
  'S'
])
param sku string = 'F0'

@description('Location (region) of this resource')
param location string = toLower(replace(resourceGroup().location, ' ', ''))

resource name_resource 'Microsoft.CognitiveServices/accounts@2017-04-18' = {
  kind: 'TextAnalytics'
  name: name
  location: location
  sku: {
    name: sku
  }
  properties: {
    customSubDomainName: name
  }
}