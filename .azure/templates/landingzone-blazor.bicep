targetScope='resourceGroup'

// Common
param location string = resourceGroup().location
param sharedSubscriptionId string = subscription().subscriptionId
param sharedResourceGroupName string
param environmentApp string 
param tags object
// Azure Monitor
param appiName string 
// Storage Account
param stName string 
param stSku string 
// App Service
param planName string 
param webName string 

module stModule '../modules/st-storageaccount.bicep' = {
  name:'stModuleName'
  params:{
    tags: tags
    location: location
    name: stName
    sku: stSku
  }
}

resource appiResource 'Microsoft.Insights/components@2020-02-02' existing = {
  name: appiName 
  scope: resourceGroup(sharedSubscriptionId, sharedResourceGroupName)
}

resource planResource 'Microsoft.Web/serverfarms@2023-01-01' existing = {
  name: planName 
  scope: resourceGroup(sharedSubscriptionId, sharedResourceGroupName)
}

module webModule '../modules/web-webapp.bicep' = {
  name: 'webModuleName'
  params:{
    name: webName
    location: location    
    tags: tags
    environment: environmentApp
    appiKey:appiResource.properties.InstrumentationKey
    appiConnection:appiResource.properties.ConnectionString
    planId: planResource.id  
  }
}
