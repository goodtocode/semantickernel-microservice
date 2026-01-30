targetScope = 'resourceGroup'

// Common
param tenantId string = tenant().tenantId
param location string = resourceGroup().location
param tags object

// Management
param sentName string
param sentSku string
param appiName string
param kvName string
param kvSku string

//
// Management
//
module sentModule '../modules/sent-loganalyticsworkspace.bicep' = {
  name: 'sentName'
  params: {
    name: sentName
    location: location
    tags: tags
    sku: sentSku
  }
}

module appiModule '../modules/appi-applicationinsights.bicep' = {
  name: 'appiName'
  params:{
    location: location
    tags: tags
    name: appiName
    workResourceId: sentModule.outputs.id
  }
}

module kvModule '../modules/kv-keyvault.bicep' = {
  name: 'kvName'
  params: {
    location: location
    tags: tags
    name: kvName
    sku: kvSku
    tenantId: tenantId
  }
} 
