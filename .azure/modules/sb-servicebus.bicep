
@description('The name of the Service Bus namespace. Must be 6-50 characters, using only alphanumeric characters and hyphens.')
@minLength(6)
@maxLength(50)
param name string

@description('The SKU (pricing tier) for the Service Bus namespace. Allowed values: Basic, Standard, Premium. Default is Basic.')
@allowed([
  'Basic'
  'Standard'
  'Premium'
])
param sku string = 'Basic'


@description('Specifies the Azure location where the Service Bus namespace should be created.')
param location string = toLower(replace(resourceGroup().location, ' ', ''))

var nameAlphanumeric = replace(replace(name, '-', ''), '.', '')

resource namespace 'Microsoft.ServiceBus/namespaces@2021-11-01' = {
  name: nameAlphanumeric
  location: location
  sku: {
    name: sku
    tier: sku
  }
  properties: {
    zoneRedundant: false
  }
}

resource authrule 'Microsoft.ServiceBus/namespaces/AuthorizationRules@2021-11-01' = {
  parent: namespace
  name: 'RootManageSharedAccessKey'
  properties: {
    rights: [
      'Listen'
      'Manage'
      'Send'
    ]
  }
}

resource netruleset 'Microsoft.ServiceBus/namespaces/networkRuleSets@2021-11-01' = if (sku == 'Premium') {
  parent: namespace
  name: 'default'
  properties: {
    defaultAction: 'Deny'
    virtualNetworkRules: []
    ipRules: []
  }
}
