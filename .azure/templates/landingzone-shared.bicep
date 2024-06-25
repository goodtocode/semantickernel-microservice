targetScope='resourceGroup'

// Common
param location string = resourceGroup().location
param tags object
// Workspace
param workName string
param workSku string
// App Service Plan
param planName string 
param planSku string 

module workModule '../modules/work-loganalyticsworkspace.bicep' = {
  name: 'logAnalyticsWorkspaceName'
  params: {
    name: workName
    location: location
    tags: tags    
    sku: workSku
  }
}

module planModule '../modules/plan-appserviceplan.bicep' = {
  name: 'appservicename'
  params: {
    name: planName
    sku: planSku
    tags: tags
    location: location    
  }
}
