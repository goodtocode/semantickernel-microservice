
@description('The name of the Bot Service resource. Must be 2-64 characters, using only letters, numbers, and hyphens, starting and ending with a letter or number.')
@minLength(2)
@maxLength(64)
param name string


@description('The SKU (pricing tier) for the Bot Service. Allowed values: F0 (Free), S1 (Standard). Default is S1.')
@allowed([
  'F0'
  'S1'
])
param sku string = 'S1'

@description('The Microsoft App ID for the Bot Service. Must be a valid GUID.')
@minLength(1)
param msAppId string

@description('The Microsoft App password/secret value for the Bot Service. Required for authentication.')
@minLength(1)
param msAppValue string

@description('The display name for the Bot Service. Optional, defaults to the resource name if not provided.')
@maxLength(64)
param displayName string = ''

@description('Tags to apply to the Bot Service resource.')
param tags object = {}

var location = resourceGroup().location
var uniqueSuffix = toLower(substring(uniqueString(resourceGroup().id, 'Microsoft.BotService/bots', name), 0, 6))
var botDisplayName = empty(displayName) ? name : displayName
var kvName = 'kv-${name}'
var appPasswordSecret = 'bot-${replace(name, '_', '-')}-pwd-${uniqueSuffix}'
var appPasswordSecretId = empty(msAppValue) ? '' : keyVaultName_appPasswordSecret.id

resource keyVaultName 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: kvName
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


resource keyVaultName_appPasswordSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = if (!empty(msAppValue)) {
  parent: keyVaultName
  name: appPasswordSecret
  properties: {
    value: msAppValue
  }
}


resource name_resource 'Microsoft.BotService/botServices@2022-09-15' = {
  name: name
  kind: 'azurebot'
  location: 'global'
  sku: {
    name: sku
  }
  tags: tags
  properties: {
    displayName: botDisplayName
    msaAppId: msAppId
    openWithHint: 'bfcomposer://'
    appPasswordHint: appPasswordSecretId
    endpoint: 'https://REPLACE-WITH-YOUR-BOT-ENDPOINT/api/messages'
  }
}
