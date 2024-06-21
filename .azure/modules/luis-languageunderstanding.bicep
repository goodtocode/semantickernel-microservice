param name string
param location string = resourceGroup().location

@allowed([
  'F0'
  'S0'
])
param sku string = 'F0'
param authoringName string

@allowed([
  'westus'
  'eastus'
])
param authoringLocation string = 'westus'

@allowed([
  'F0'
])
param authoringSku string = 'F0'

resource name_resource 'Microsoft.CognitiveServices/accounts@2017-04-18' = {
  name: name
  location: location
  kind: 'LUIS'
  sku: {
    name: sku
  }
  properties: {
    customSubDomainName: name
  }
}

resource authoringName_resource 'Microsoft.CognitiveServices/accounts@2017-04-18' = {
  name: authoringName
  location: authoringLocation
  kind: 'LUIS.Authoring'
  sku: {
    name: authoringSku
  }
  properties: {
    customSubDomainName: authoringName
  }
  dependsOn: [
    name_resource
  ]
}