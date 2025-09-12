param name string 
param location string 
param sku string 
param tenantId string 
param tags object = {}
param accessPolicies array = []
param enableRbacAuthorization bool = true

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
    accessPolicies: accessPolicies == [] && enableRbacAuthorization == true ? null : accessPolicies
    enableRbacAuthorization: enableRbacAuthorization
    networkAcls: {
      defaultAction: 'Allow'
      bypass: 'AzureServices'
      virtualNetworkRules: []
      ipRules: [] 
    }
  }
} 

output id string = kvResource.id
