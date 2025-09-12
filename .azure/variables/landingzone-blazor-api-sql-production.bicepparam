using '../templates/landingzone-blazor-api-sql.bicep'
// Common
var organizationName = 'COMPANY'
var productName = 'PRODUCT'
var environmentIac = 'prod'
param environmentApp = 'Production'
param location = 'West US 2'
param tags = { Environment: environmentIac, CostCenter: '0000' }

// Workspace
param sharedResourceGroupName = '${organizationName}-rg-shared-${environmentIac}-001'

// Azure Monitor
param appiName = 'appi-${productName}-${environmentIac}-001'

// Storage
param stName = 'st${productName}${environmentIac}001'
param stSku = 'Standard_LRS'

// App Service
var planSku = 'F1'
param webName = 'web-${productName}-${environmentIac}-001'
param apiName = 'api-${productName}-${environmentIac}-001'
param planName = 'plan-shared-${planSku}-${environmentIac}-001'

// SQL Server
param sqlName = 'sql-${productName}-${environmentIac}-001'
param sqlAdminUser = ''
param sqlAdminPassword = ''
param sqldbName = 'sqldb-${productName}-${environmentIac}-001'
param sqldbSku = 'Basic'
