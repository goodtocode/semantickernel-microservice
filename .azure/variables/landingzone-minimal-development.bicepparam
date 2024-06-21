using '../templates/landingzone-minimal.bicep'
// Common
param tenantId = 'TENANT_ID'
param location = 'West US 2'
param tags = { Environment: 'dev', CostCenter: '0000' }
// Workspace
param sharedSubscriptionId = 'SUBSCRIPTION_ID'
param sharedResourceGroupName = 'rg-shared-dev-001'
param workName = 'work-shared-dev-001'

// Azure Monitor
param appiName = 'appi-landingzone-dev-001'
param Flow_Type = 'Bluefield'
param Application_Type = 'web'

// Storage
param stName = 'stPRODUCTdev001'
param stSku = 'Standard_LRS'

// Key Vault
param kvName = 'kv-PRODUCT-dev-001'
param kvSku = 'standard'
