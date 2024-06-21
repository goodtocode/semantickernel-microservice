using '../templates/landingzone-standalone.bicep'

// Common
param tenantId = 'TENANT_ID'
param location = 'West US 2'
param tags = { Environment: 'dev', CostCenter: '0000' }
// Workspace
param workName = 'work-SHARED-dev-001'
param workSku = 'PerGB2018'

// Azure Monitor
param appiName = 'appi-PRODUCT-dev-001'
param Flow_Type = 'Bluefield'
param skuName = 'standard'  
param Application_Type = 'web'

// Storage
param storageName = 'stPRODUCTdev001'
param storageSkuName = 'Standard_LRS'

// Key Vault
param keyVaultName = 'kv-PRODUCT-dev-001'
