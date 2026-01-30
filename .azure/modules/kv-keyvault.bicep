
@description('The name of the Key Vault. Must be 3-24 characters, using only alphanumeric characters and hyphens.')
@minLength(3)
@maxLength(24)
param name string

@description('The Azure region where the Key Vault will be deployed.')
param location string = resourceGroup().location

@description('The SKU (pricing tier) for the Key Vault. Allowed values: standard, premium. Default is standard.')
@allowed([
  'standard'
  'premium'
])
param sku string = 'standard'

@description('The tenant ID of the Azure Active Directory that will be used for authentication.')
@minLength(36)
@maxLength(36)
param tenantId string = tenant().tenantId

@description('Tags to apply to the Key Vault resource.')
param tags object = {}

@description('Access policies to assign to the Key Vault.')
param accessPolicies array = []

@description('Enable RBAC authorization for the Key Vault. Default is true.')
param enableRbacAuthorization bool = true

@description('List of allowed IP addresses for Key Vault access. Default is empty (no IPs allowed).')
param allowedIpRules array = []

@description('List of allowed Virtual Network resource IDs for Key Vault access. Default is empty (no VNets allowed).')
param allowedVirtualNetworkResourceIds array = []

@description('Enable soft delete for the Key Vault. Default is true.')
param enableSoftDelete bool = true

@description('Enable purge protection for the Key Vault. Default is true.')
param enablePurgeProtection bool = true

resource kvResource 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: name
  location: location
  tags: empty(tags) ? null : tags
  properties: {
    enabledForDeployment: true
    enabledForDiskEncryption: true
    enabledForTemplateDeployment: true
    tenantId: tenantId
    publicNetworkAccess: 'Disabled'
    sku: {
      name: sku
      family: 'A'
    }    
    accessPolicies: accessPolicies == [] && enableRbacAuthorization == true ? null : accessPolicies
    enableRbacAuthorization: enableRbacAuthorization
    enableSoftDelete: enableSoftDelete
    enablePurgeProtection: enablePurgeProtection
    networkAcls: {
      defaultAction: 'Deny'
      bypass: 'AzureServices'
      virtualNetworkRules: allowedVirtualNetworkResourceIds
      ipRules: allowedIpRules
    }
  }
} 

output id string = kvResource.id
