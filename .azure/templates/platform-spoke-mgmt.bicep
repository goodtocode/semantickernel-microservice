
targetScope = 'resourceGroup'

param tenantId string = tenant().tenantId
param location string = resourceGroup().location
param tags object

@minLength(1)
@maxLength(64)
@description('Specifies the subscription ID for the hub management. Must not be empty.')
param hubMgmtSubscriptionId string = subscription().subscriptionId

@minLength(1)
@maxLength(90)
@description('Specifies the resource group name for hub management. Must not be empty. 1-90 characters, letters, numbers, -, _, (, ) and .')
param hubMgmtResourceGroupName string

@minLength(4)
@maxLength(63)
@description('Specifies the name of the Log Analytics workspace. 4-63 characters, letters, numbers, and -')
param workName string

@minLength(1)
@maxLength(255)
@description('Specifies the name of the Application Insights resource. 1-255 characters, letters, numbers, and -')
param appiName string

@minLength(5)
@maxLength(50)
@description('Specifies the name of the App Configuration store. 5-50 characters, only lowercase letters, numbers, and -')
param appcsName string
param appcsSku string = 'free'

// App Service Plan parameters
param planSku string = 'F1'
param planName string

// '-appcs-kv' is 9 chars, so truncate base to 15 chars max for 24-char total
var kvNameBase = toLower(replace(appcsName, '-', ''))
var kvNameTrunc = substring(kvNameBase, 0, min(15, length(kvNameBase)))
var kvName = '${kvNameTrunc}-appcs-kv'
var kvSku = 'standard'

resource workResource 'Microsoft.OperationalInsights/workspaces@2023-09-01' existing = {
  name: workName 
  scope: resourceGroup(hubMgmtSubscriptionId, hubMgmtResourceGroupName)
}

module appiModule '../modules/appi-applicationinsights.bicep' = {
  name: 'appiName'
  params:{
    location: location
    tags: tags
    name: appiName
    workResourceId: workResource.id
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

module appcsModule '../modules/appcs-appconfigurationstore.bicep' = {
  name: 'appcsName'
  params: {
    name: appcsName
    sku: appcsSku
    location: location
  }
}

module planModule '../modules/plan-appserviceplan.bicep' = {
  name: 'planModule'
  params: {
    name: planName
    sku: planSku
    location: location
  }
}


