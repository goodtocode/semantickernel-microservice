param name string
param location string = resourceGroup().location

@description('Describes plan\'s pricing tier and capacity. Check details at https://azure.microsoft.com/en-us/pricing/details/app-service/')
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

resource name_RootManageSharedAccessKey 'Microsoft.Relay/namespaces/AuthorizationRules@2017-04-01' = {
  parent: name_resource
  name: 'RootManageSharedAccessKey'
  location: location
  properties: {
    rights: [
      'Listen'
      'Manage'
      'Send'
    ]
  }
}

resource name_default 'Microsoft.Relay/namespaces/networkRuleSets@2018-01-01-preview' = {
  parent: name_resource
  name: 'default'
  location: location
  properties: {
    defaultAction: 'Deny'
    ipRules: []
  }
}