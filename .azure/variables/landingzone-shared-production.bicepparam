using '../templates/landingzone-shared.bicep'

// Common
param location = 'West US 2'
param tags = { Environment: 'prod', CostCenter: '0000' }

// Workspace
param workName = 'work-SHARED-prod-001'
param workSku = 'PerGB2018'

// App Service
param planName = 'plan-PRODUCT-prod-001'
param planSku = 'S1'
