@description('That name is the name of our application. It has to be unique.Type a name followed by your resource group name. (<name>-<resourceGroupName>)')
param name string = 'CognitiveService-${uniqueString(resourceGroup().id)}'

@allowed([
  'S0'
])
param sku string = 'S0'

resource name_resource 'Microsoft.CognitiveServices/accounts@2017-04-18' = {
  name: name
  location: resourceGroup().location
  sku: {
    name: sku
  }
  kind: 'CognitiveServices'
  properties: {
    statisticsEnabled: false
  }
}