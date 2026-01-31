using '../templates/landingzone-standalone-web-api-sql.bicep'

// Common
var productIac = 'semker'
var environmentIac = 'dev'
var regionIac = 'wus2'
var instanceIac = '001'
var planSku = 'F1'

param environmentApp = 'Development'
param location = 'westus2'
param tags = { Environment: environmentIac, CostCenter: '0000' }

// Common Services
param appiName = '${productIac}-${environmentIac}-${regionIac}-${instanceIac}-appi'
param workName = '${productIac}-${environmentIac}-${regionIac}-${instanceIac}-appi'


// App Service
param webName = '${productIac}-${environmentIac}-${regionIac}-${instanceIac}-web'
param apiName = '${productIac}-${environmentIac}-${regionIac}-${instanceIac}-api'
param planName = '${productIac}-${environmentIac}-${regionIac}-${planSku}-${instanceIac}-plan'

// SQL Server
param sqlName = '${productIac}-${environmentIac}-${regionIac}-${instanceIac}-sql'
param sqlAdminUser = 'LocalAdmin'
param sqlAdminPassword = 'PASS_FROM_CLI_PARAMETERS'
param sqldbName = '${productIac}-${environmentIac}-${regionIac}-${instanceIac}-sqldb'
param sqldbSku = 'Basic'
