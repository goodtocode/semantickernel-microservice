
@description('The name of the Cognitive Services account. Must be unique within Azure. Recommended format: <name>-<resourceGroupName>. 3-64 characters, lowercase letters, numbers, and hyphens.')
@minLength(3)
@maxLength(64)
param name string = 'CognitiveService-${uniqueString(resourceGroup().id)}'


@description('The SKU (pricing tier) for the Cognitive Services account. Allowed values: S0 (Standard). Default is S0.')
@allowed([
  'S0'
])
param sku string = 'S0'

resource name_resource 'Microsoft.CognitiveServices/accounts@2023-05-01' = {
  name: name
  location: resourceGroup().location
  sku: {
    name: sku
  }
  kind: 'CognitiveServices'
}
