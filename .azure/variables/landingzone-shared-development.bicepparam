using '../templates/landingzone-shared.bicep'

// Common
param location = 'West US 2'
param tags = { Environment: 'dev', CostCenter: '0000' }

// Workspace
param workName = 'work-shared-westus2-dev-001'
param workSku = 'PerGB2018'

// App Service
param planName = 'plan-shared-westus2-dev-001'
param planSku = 'F1'
