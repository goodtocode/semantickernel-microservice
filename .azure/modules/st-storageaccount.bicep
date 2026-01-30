
@description('The Azure region where the storage account will be deployed.')
param location string

@description('Resource tags to apply to the storage account.')
param tags object = {}

@description('The name of the storage account. Must be globally unique, 3-24 characters, using only lowercase letters and numbers.')
@minLength(3)
@maxLength(24)
param name string

@description('The SKU (pricing tier) of the storage account. Allowed values: Standard_LRS, Standard_GRS, Standard_RAGRS, Standard_ZRS, Premium_LRS, Premium_ZRS. Default is Standard_LRS.')
@allowed([
  'Standard_LRS'
  'Standard_GRS'
  'Standard_RAGRS'
  'Standard_ZRS'
  'Premium_LRS'
  'Premium_ZRS'
])
param sku string = 'Standard_LRS'


@description('List of allowed IP addresses for Storage Account access. Default is empty (no IPs allowed).')
param allowedIpRules array = []

resource stResource 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: name
  location: location
  tags: empty(tags) ? null : tags
  sku: {
    name: sku
  }
  kind: 'StorageV2'
  properties: {
    allowBlobPublicAccess: false
    supportsHttpsTrafficOnly: true
    minimumTlsVersion: 'TLS1_2'
    allowSharedKeyAccess: false
    publicNetworkAccess: 'Disabled'
    networkAcls: {
      defaultAction: 'Deny'
      bypass: 'AzureServices'
      ipRules: allowedIpRules
      virtualNetworkRules: []
    }
    encryption: {
      keySource: 'Microsoft.Storage'
      requireInfrastructureEncryption: true
      services: {
        blob: {
          enabled: true
          keyType: 'Account'
        }
        file: {
          enabled: true
          keyType: 'Account'
        }
        queue: {
          enabled: true
          keyType: 'Service'
        }
        table: {
          enabled: true
          keyType: 'Service'
        }
      }
    }
  }
}

output id string = stResource.id
