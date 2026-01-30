using '../templates/landingzone-web-api-sql.bicep'

// Common
var tenantIac = 'can'
var productIac = 'semker'
var environmentIac = 'dev'
var regionIac = 'wus2'
var instanceIac = '001'
var planSku = 'F1'

param environmentApp = 'Development'
param location = 'westus2'
param tags = { Environment: environmentIac, CostCenter: '0000' }

// Mgmt Resource Group (spoke)
param spokeMgmtResourceGroupName = '${tenantIac}-spokemgmt-${environmentIac}-${regionIac}-${instanceIac}-rg'
param appiName = 'spokemgmt-${environmentIac}-${regionIac}-${instanceIac}-appi'

// App Service
param webName = '${productIac}-${environmentIac}-${regionIac}-${instanceIac}-web'
param apiName = '${productIac}-${environmentIac}-${regionIac}-${instanceIac}-api'
param planName = 'spokemgmt-${environmentIac}-${regionIac}-${planSku}-${instanceIac}-plan'

// SQL Server
param sqlName = '${productIac}-${environmentIac}-${regionIac}-${instanceIac}-sql'
param sqlAdminUser = ''
param sqlAdminPassword = ''
param sqldbName = '${productIac}-${environmentIac}-${regionIac}-${instanceIac}-sqldb'
param sqldbSku = 'Basic'
