param name string
param sku string = 'Basic'

@description('Specifies the Azure location where the resource should be created.')
param location string = toLower(replace(resourceGroup().location, ' ', ''))

var nameAlphaNumeric_var = replace(replace(name, '-', ''), '.', '')

resource nameAlphaNumeric 'Microsoft.ServiceBus/namespaces@2021-01-01-preview' = {
  name: nameAlphaNumeric_var
  location: location
  sku: {
    name: sku
    tier: sku
  }
  properties: {
    zoneRedundant: false
  }
}

resource nameAlphaNumeric_RootManageSharedAccessKey 'Microsoft.ServiceBus/namespaces/AuthorizationRules@2021-01-01-preview' = {
  parent: nameAlphaNumeric
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

resource nameAlphaNumeric_default 'Microsoft.ServiceBus/namespaces/networkRuleSets@2021-01-01-preview' = if (sku == 'Premium') {
  parent: nameAlphaNumeric
  name: 'default'
  location: location
  properties: {
    defaultAction: 'Deny'
    virtualNetworkRules: []
    ipRules: []
  }
}