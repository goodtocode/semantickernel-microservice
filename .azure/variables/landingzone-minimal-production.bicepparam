using '../templates/landingzone-minimal.bicep'
// Common
//param tenantId = 'TENANT_ID'
param location = 'West US 2'
param tags = { Environment: 'prod', CostCenter: '0000' }
// Workspace
//param sharedSubscriptionId = 'SUBSCRIPTION_ID'
param sharedResourceGroupName = 'rg-SHARED-prod-001'
param workName = 'work-SHARED-prod-001'

// Azure Monitor
param appiName = 'appi-semantickernel-prod-001'
param Flow_Type = 'Bluefield'
param Application_Type = 'web'

// Storage
param stName = 'stsemantickernelprod001'
param stSku = 'Standard_LRS'

// Key Vault
param kvName = 'kv-semantickernel-prod-001'
param kvSku = 'standard'
