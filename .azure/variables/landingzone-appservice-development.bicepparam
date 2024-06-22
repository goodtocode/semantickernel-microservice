using '../templates/landingzone-appservice.bicep'
// Common
//param tenantId = 'TENANT_ID'
param rgEnvironment = 'Development'
param location = 'West US 2'
param tags = { Environment: 'dev', CostCenter: '0000' }
// Workspace
//param sharedSubscriptionId = 'SUBSCRIPTION_ID'
param sharedResourceGroupName = 'rg-SHARED-dev-001'
param workName = 'work-SHARED-dev-001'

// Azure Monitor
param appiName = 'appi-semantickernel-dev-001'
param Flow_Type = 'Bluefield'
param Application_Type = 'web'

// Storage
param stName = 'stsemantickerneldev001'
param stSku = 'Standard_LRS'

// Key Vault
param kvName = 'kv-semantickernel-dev-001'
param kvSku = 'standard'

// App Service
param appName = 'api-semantickernel-dev-001'
param planName = 'plan-SHARED-dev-001'
