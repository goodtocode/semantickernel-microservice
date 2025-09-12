targetScope='resourceGroup'

// Common
param tenantId string = tenant().tenantId
param location string = resourceGroup().location
param sharedSubscriptionId string = subscription().subscriptionId
param sharedResourceGroupName string
param tags object
// Azure Monitor
param appiName string 
param Application_Type string 
param Flow_Type string 
// Key Vault
param kvName string 
param kvSku string 
// Storage Account
param stName string 
param stSku string 
// Static Web App
param stappName string
param repositoryUrl string
param branch string = 'main'
// workspace
param workName string

resource workResource 'Microsoft.OperationalInsights/workspaces@2023-09-01' existing = {
  name: workName 
  scope: resourceGroup(sharedSubscriptionId, sharedResourceGroupName)
}

module appiModule '../modules/appi-applicationinsights.bicep' = {
  name: 'appiModuleName'
  params:{
    location: location
    tags: tags
    name: appiName
    Application_Type: Application_Type
    Flow_Type: Flow_Type
    workResourceId: workResource.id
  }
}

module kvModule '../modules/kv-keyvault.bicep'= {
   name:'kvModuleName'
   params:{
    location: location
    tags: tags
    name: kvName
    sku: kvSku
    tenantId: tenantId
   }
}

module stModule '../modules/st-storageaccount.bicep' = {
  name:'stModuleName'
  params:{
    tags: tags
    location: location
    name: stName
    sku: stSku
  }
}

module apiModule '../modules/stapp-staticwebapp.bicep' = {
  name: 'stappModuleName'
  params:{
    name: stappName
    location: location    
    tags: tags
    repositoryUrl: repositoryUrl
    branch: branch
  }
}
