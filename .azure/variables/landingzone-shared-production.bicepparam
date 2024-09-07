using '../templates/landingzone-shared.bicep'

// Common
var environmentIac = 'prod'
param location = 'West US 2'
param tags = { Environment: environmentIac, CostCenter: '0000' }

// Workspace
param workName = 'work-shared-${environmentIac}-001'
param workSku = 'PerGB2018'

// App Service
param planSku = 'F1'
param planName = 'plan-shared-${planSku}-${environmentIac}-001'
