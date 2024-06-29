using '../templates/landingzone-appservicesql.bicep'
// Common
param rgEnvironment = 'Production'
param location = 'West US 2'
param tags = { Environment: 'prod', CostCenter: '0000' }
// Workspace
param sharedResourceGroupName = 'gtc-rg-shared-westus2-prod-001'
param workName = 'work-shared-westus2-prod-001'

// Azure Monitor
param appiName = 'appi-semantickernel-prod-001'
param Flow_Type = 'Bluefield'
param Application_Type = 'web'

// Storage
param stName = 'stsemantickernelprod001'
param stSku = 'Standard_LRS'

// Key Vault
param kvName = 'kv-semanticker-prod-001'
param kvSku = 'standard'

// App Service
param appName = 'api-semantickernel-prod-001'
param planName = 'plan-shared-westus2-prod-001'

// SQL Server
// SQL Server
param sqlName = 'sql-semantickernel-prod-001'
param sqlAdminUser = ''
param sqlAdminPassword = ''
param sqldbName = 'sqldb-semantickernel-prod-001'
param sqldbSku = 'Basic'
