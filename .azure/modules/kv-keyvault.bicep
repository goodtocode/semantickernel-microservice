param name string 
param location string 
param sku string 
param tenantId string 
param tags object = {}

resource kvResource 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: name
  location: location
  tags: empty(tags) ? null : tags
  properties: {
    enabledForDeployment: true
    enabledForDiskEncryption: true
    enabledForTemplateDeployment: true
    tenantId: tenantId
    publicNetworkAccess: 'Enabled'
    sku: {
      name: sku
      family: 'A'
    }    
    enableRbacAuthorization: true
    networkAcls: {
      defaultAction: 'Allow'
      bypass: 'AzureServices'
      virtualNetworkRules: []
      ipRules: [] 
    }
  }
} 

output id string = kvResource.id
