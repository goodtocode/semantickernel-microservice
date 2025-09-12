targetScope='resourceGroup'

// Common
param tags object
param location string
// Workspace
param workName string
param workSku string
// App Service Plan
param planName string 
param planSku string 

module workModule '../modules/work-loganalyticsworkspace.bicep' = {
  name: 'workModuleName'
  params: {
    name: workName
    location: location
    tags: tags   
    sku: workSku
  }
}

module planModule '../modules/plan-appserviceplan.bicep' = {
  name: 'planModuleName'
  params: {
    name: planName
    sku: planSku
    tags: tags
    location: location    
  }
}
