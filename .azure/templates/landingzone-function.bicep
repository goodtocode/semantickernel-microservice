targetScope='resourceGroup'

// Common
param tenantId string 
param location string 
param tags object 
param environmentApp string 
param sharedSubscriptionId string
param sharedResourceGroupName string
// Azure Monitor
param appiName string 
param Application_Type string 
param Flow_Type string 
// Key Vault
param kvName string 
param kvSku string 
param accessPolicies array 
// Storage Account
param stName string 
param stSku string 
// workspace
param workName string
// function
param funcName string
param planName string
param alwaysOn bool = false


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
    accessPolicies: accessPolicies
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

resource planResource 'Microsoft.Web/serverfarms@2023-01-01' existing = {
  name: planName 
  scope: resourceGroup(sharedSubscriptionId, sharedResourceGroupName)
}

module funcModule '../modules/func-functionsapp.bicep' = {
  name: 'funcModuleName'
  params:{
    name: funcName
    location: location    
    tags: tags
    environmentApp: environmentApp
    appiKey:appiModule.outputs.InstrumentationKey
    appiConnection:appiModule.outputs.Connectionstring
    planId: planResource.id
    stName: stName
    alwaysOn: alwaysOn
  }
}
