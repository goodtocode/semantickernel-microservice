
@description('The name of the LUIS Cognitive Service resource. Must be 2-64 characters, using only alphanumeric characters and hyphens.')
@minLength(2)
@maxLength(64)
param name string

@description('The Azure region where the LUIS Cognitive Service resource will be deployed.')
param location string = resourceGroup().location


@description('The SKU (pricing tier) for the LUIS Cognitive Service resource. Allowed values: F0, S0. Default is F0.')
@allowed([
  'F0'
  'S0'
])
param sku string = 'F0'

@description('The name of the LUIS Authoring resource. Must be 2-64 characters, using only alphanumeric characters and hyphens.')
@minLength(2)
@maxLength(64)
param authoringName string


@description('The Azure region for the LUIS Authoring resource. Allowed values: westus, eastus. Default is westus.')
@allowed([
  'westus'
  'eastus'
])
param authoringLocation string = 'westus'


@description('The SKU (pricing tier) for the LUIS Authoring resource. Allowed value: F0. Default is F0.')
@allowed([
  'F0'
])
param authoringSku string = 'F0'

resource name_resource 'Microsoft.CognitiveServices/accounts@2023-05-01' = {
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

resource authoringName_resource 'Microsoft.CognitiveServices/accounts@2023-05-01' = {
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
