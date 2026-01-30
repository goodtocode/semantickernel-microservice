
@description('The name of the Azure Relay namespace. Must be 6-50 characters, using only alphanumeric characters and hyphens.')
@minLength(6)
@maxLength(50)
param name string

@description('The Azure region where the Relay namespace will be deployed.')
param location string = resourceGroup().location

@description('The SKU (pricing tier) for the Azure Relay namespace. Allowed value: Standard. Default is Standard.')
@allowed([
  'Standard'
])
param sku string = 'Standard'

resource name_resource 'Microsoft.Relay/namespaces@2018-01-01-preview' = {
  name: name
  location: location
  sku: {
    name: sku
    tier: sku
  }
  properties: {}
}

resource name_RootManageSharedAccessKey 'Microsoft.Relay/namespaces/authorizationRules@2021-11-01' = {
  parent: name_resource
  name: 'RootManageSharedAccessKey'
  properties: {
    rights: [
      'Listen'
      'Manage'
      'Send'
    ]
  }
}

resource name_default 'Microsoft.Relay/namespaces/networkRuleSets@2021-11-01' = {
  parent: name_resource
  name: 'default'
  properties: {
    defaultAction: 'Deny'
    ipRules: []
  }
}
