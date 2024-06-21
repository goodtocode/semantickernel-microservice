using '../templates/landingzone-appservice.bicep'
// Common
param tenantId = 'TENANT_ID'
param rgEnvironment = 'Production'
param location = 'West US 2'
param tags = { Environment: 'prod', CostCenter: '0000' }
// Workspace
param sharedSubscriptionId = 'SUBSCRIPTION_ID'
param sharedResourceGroupName = 'rg-SHARED-prod-001'
param workName = 'work-SHARED-prod-001'

// Azure Monitor
param appiName = 'appi-PRODUCT-prod-001'
param Flow_Type = 'Bluefield'
param Application_Type = 'web'

// Storage
param stName = 'stPRODUCTprod001'
param stSku = 'Standard_LRS'

// Key Vault
param kvName = 'kv-PRODUCT-prod-001'
param kvSku = 'standard'

// App Service
param appName = 'api-PRODUCT-prod-001'
param planName = 'plan-SHARED-prod-001'
