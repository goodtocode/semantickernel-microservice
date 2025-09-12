using '../templates/landingzone-appservicesql.bicep'
// Common
var organizationName = 'gtc'
var productName = 'semkernel'
var subscriptionName = 'production'
param environmentApp = 'Production'
var environmentIac = 'prod'
param location = 'West US 2'
param tags = { Environment: environmentIac, CostCenter: '0000' }
// Workspace
param sharedResourceGroupName = '${organizationName}-rg-${subscriptionName}-shared-${environmentIac}-001'
param workName = 'work-shared-${environmentIac}-001'

// Azure Monitor
param appiName = 'appi-${productName}-${environmentIac}-001'
param Flow_Type = 'Bluefield'
param Application_Type = 'web'

// Storage
param stName = 'st${productName}${environmentIac}001'
param stSku = 'Standard_LRS'

// Key Vault
param kvName = 'kv-${productName}-${environmentIac}-002'
param kvSku = 'standard'

// App Service
var planSku = 'F1'
param appName = 'api-${productName}-${environmentIac}-001'
param planName = 'plan-shared-${planSku}-${environmentIac}-001'

// SQL Server
param sqlName = 'sql-${productName}-${environmentIac}-001'
param sqlAdminUser = ''
param sqlAdminPassword = ''
param sqldbName = 'sqldb-${productName}-${environmentIac}-001'
param sqldbSku = 'Basic'
