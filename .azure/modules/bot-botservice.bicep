param name string

@allowed([
  'F0'
  'S1'
])
param sku string = 'S1'
param msAppId string
param msAppValue string
param displayName string = ''
param resourceTags object = {
  'Microsoft.BotService/botServices': {}
}

var location = resourceGroup().location
var uniqueSuffix = toLower(substring(uniqueString(resourceGroup().id, 'Microsoft.BotService/bots', name), 0, 6))
var botDisplayName = (empty(displayName) ? name : displayName)
var keyVaultName_var = 'kv-${name}'
var appPasswordSecret = 'bot-${replace(name, '_', '-')}-pwd-${uniqueSuffix}'
var appPasswordSecretId = keyVaultName_appPasswordSecret.id
var empty = {}
var botTags = (contains(resourceTags, 'Microsoft.BotService/botServices') ? resourceTags.Microsoft.BotService / botServices : empty)

resource keyVaultName 'Microsoft.KeyVault/vaults@2019-09-01' = {
  name: keyVaultName_var
  location: location
  properties: {
    tenantId: subscription().tenantId
    sku: {
      family: 'A'
      name: 'standard'
    }
    accessPolicies: []
    enabledForTemplateDeployment: true
  }
}

resource keyVaultName_appPasswordSecret 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = if (!empty(msAppValue)) {
  parent: keyVaultName
  name: '${appPasswordSecret}'
  properties: {
    value: msAppValue
  }
}

resource name_resource 'Microsoft.BotService/botServices@2018-07-12' = {
  name: name
  kind: 'azurebot'
  location: 'global'
  sku: {
    name: sku
  }
  tags: botTags
  properties: {
    displayName: botDisplayName
    msaAppId: msAppId
    openWithHint: 'bfcomposer://'
    appPasswordHint: appPasswordSecretId
  }
  dependsOn: []
}

resource keyVaultName_appPasswordSecret_Microsoft_Resources_provisioned_for 'Microsoft.KeyVault/vaults/secrets/providers/links@2018-02-01' = {
  name: '${keyVaultName_var}/${appPasswordSecret}/Microsoft.Resources/provisioned-for'
  location: location
  properties: {
    targetId: resourceId('Microsoft.BotService/bots', name)
    sourceId: appPasswordSecretId
  }
  dependsOn: [
    keyVaultName
    name_resource
  ]
}