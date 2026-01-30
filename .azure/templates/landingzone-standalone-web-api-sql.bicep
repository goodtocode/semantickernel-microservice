targetScope = 'resourceGroup'

@description('The Azure region where resources will be deployed. Only allowed regions for SQL module.')
@allowed([
  'eastus'
  'eastus2'
  'centralus'
  'westus'
  'westus2'
])
param location string = 'eastus'

@description('Resource tags to be applied to all resources.')
param tags object

@minLength(4)
@maxLength(63)
@description('Specifies the name of the Log Analytics workspace. 4-63 characters, letters, numbers, and -')
param workName string

@allowed(['PerGB2018', 'Free'])
@description('SKU for the Log Analytics workspace. Allowed: PerGB2018, Free.')
param workSku string = 'PerGB2018'

@minLength(1)
@maxLength(255)
@description('Specifies the name of the Application Insights resource. 1-255 characters, letters, numbers, and -')
param appiName string

@allowed(['F1', 'B1', 'B2', 'B3', 'D1', 'P1', 'P2', 'P3', 'P4', 'S1', 'S2', 'S3', 'Y1'])
@description('SKU for the App Service Plan. Allowed: F1, B1, B2, B3, D1, P1, P2, P3, P4, S1, S2, S3, Y1.')
param planSku string = 'F1'

@minLength(1)
@maxLength(40)
@description('Name of the App Service Plan. 1-40 characters.')
param planName string

@minLength(1)
@maxLength(40)
@description('Environment name for the application. 1-40 characters.')
param environmentApp string

@minLength(1)
@maxLength(60)
@description('Name of the Web App. 1-60 characters.')
param webName string

@minLength(1)
@maxLength(60)
@description('Name of the API App. 1-60 characters.')
param apiName string

@minLength(1)
@maxLength(60)
@description('Name of the SQL Server. 1-60 characters.')
param sqlName string

@minLength(1)
@maxLength(60)
@description('SQL Server admin username. 1-60 characters.')
param sqlAdminUser string

@secure()
@minLength(8)
@maxLength(60)
@description('SQL Server admin password. 8-60 characters.')
param sqlAdminPassword string

@minLength(1)
@maxLength(60)
@description('Name of the SQL Database. 1-60 characters.')
param sqldbName string

@allowed(['Basic', 'Premium', 'Standard'])
@description('SKU for the SQL Database. Allowed: Basic, Premium, Standard.')
param sqldbSku string = 'Basic'

module workModule '../modules/sent-loganalyticsworkspace.bicep' = {
  name: 'workName'
  params: {
    name: workName
    location: location
    tags: tags
    sku: workSku
  }
}

module appiModule '../modules/appi-applicationinsights.bicep' = {
  name: 'appiName'
  params:{
    location: location
    tags: tags
    name: appiName
    workResourceId: workModule.outputs.id
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

module apiModule '../modules/api-appservice.bicep' = {
  name: 'apiModuleName'
  params:{
    name: apiName
    location: location    
    tags: tags
    environment: environmentApp
    appiKey:appiModule.outputs.InstrumentationKey
    appiConnection:appiModule.outputs.Connectionstring
    planId: planModule.outputs.id  
  }
}

module webModule '../modules/web-appservice.bicep' = {
  name: 'webModuleName'
  params:{
    name: webName
    location: location    
    tags: tags
    environment: environmentApp
    appiKey:appiModule.outputs.InstrumentationKey
    appiConnection:appiModule.outputs.Connectionstring
    planId: planModule.outputs.id  
  }
}

module sqlModule '../modules/sql-sqlserverdatabase.bicep' = {
  name: 'sqlModuleName'
  params:{
    name: sqlName
    location: location    
    tags: tags    
    adminLogin: sqlAdminUser
    adminPassword: sqlAdminPassword
    sqldbName: sqldbName
    sku: sqldbSku
  }
}
