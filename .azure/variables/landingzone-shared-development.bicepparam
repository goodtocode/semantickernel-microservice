using '../templates/landingzone-shared.bicep'

// Common
param location = 'West US 2'
param tags = { Environment: 'dev', CostCenter: '0000' }

// Workspace
param workName = 'work-shared-dev-001'
param workSku = 'PerGB2018'

// App Service
param planName = 'plan-shared-dev-001'
param planSku = 'S1'
