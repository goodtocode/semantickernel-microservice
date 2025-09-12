using '../templates/landingzone-stapp.bicep'
// Common
var organizationName = 'gtc'
var productName = 'PRODUCT'
var environmentIac = 'dev'
param location = 'West US 2'
param tags = { Environment: environmentIac, CostCenter: '0000' }
// Workspace
param sharedResourceGroupName = '${organizationName}-rg-shared-${environmentIac}-001'
param workName = 'work-shared-${environmentIac}-001'

// Azure Monitor
param appiName = 'appi-${productName}-${environmentIac}-001'
param Flow_Type = 'Bluefield'
param Application_Type = 'web'

// Storage
param stName = 'st${productName}${environmentIac}001'
param stSku = 'Standard_LRS'

// Key Vault
param kvName = 'kv-${productName}-${environmentIac}-001'
param kvSku = 'standard'

// App Service
param stappName = 'stapp-${productName}-${environmentIac}-001'
param repositoryUrl = ''
