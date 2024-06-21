targetScope='resourceGroup'

// Common
param tenantId string 
param location string 
param tags object 
param sharedSubscriptionId string
param sharedResourceGroupName string
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
// workspace
param workName string

resource workResource 'Microsoft.OperationalInsights/workspaces@2023-09-01' existing = {
  name: workName 
  scope: resourceGroup(sharedSubscriptionId, sharedResourceGroupName)
}

module appiModule '../modules/appi-applicationinsights.bicep' = {
  name: 'appiName'
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
   name:'kvName'
   params:{
    location: location
    tags: tags
    name: kvName
    sku: kvSku
    tenantId: tenantId
   }
}

module stModule '../modules/st-storageaccount.bicep' = {
  name:'storagename'
  params:{
    tags: tags
    location: location
    name: stName
    sku: stSku
  }
}
