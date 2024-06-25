// Sql Server
@minLength(1)
@maxLength(60)
param name string
param location string = resourceGroup().location
param tags object
@minLength(1)
@maxLength(60)
param adminLogin string
@minLength(1)
@maxLength(128)
@secure()
param adminPassword string
param startIpAddress string = '0.0.0.0'
param endIpAddress string = '0.0.0.0'
// Sql Database
@minLength(1)
@maxLength(60)
param sqldbName string
param collation string = 'SQL_Latin1_General_CP1_CI_AS'
@allowed([
  'Basic'
  'Standard'
  'Premium'
])
param sku string = 'Basic'
param maxSizeBytes int = 1073741824

resource sqlServer 'Microsoft.Sql/servers@2023-08-01-preview' = {
  name: name
  location: location
  tags: tags
  properties: {
    administratorLogin: adminLogin
    administratorLoginPassword: adminPassword
  }
}

resource sqlServerFirewall 'Microsoft.Sql/servers/firewallRules@2023-08-01-preview' = {
  parent: sqlServer
  name: 'AllowAllWindowsAzureIps'
  properties: {
    endIpAddress: endIpAddress
    startIpAddress: startIpAddress
  }
}

output id string = sqlServer.id
output name string = sqlServer.name

resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-05-01-preview' = {
  parent: sqlServer
  name: sqldbName
  location: location
  tags: {
    displayName: 'Database'
  }
  sku: {
    name: sku
    tier: sku // Replace with the desired SKU tier (e.g., Basic, GeneralPurpose, BusinessCritical)
    //family: 'skuFamily' // Replace with the desired SKU family (e.g., Gen4, Gen5)
    capacity: 1 // Replace with the desired capacity (e.g., 1, 2, 4)
  }
  properties: {
    collation: collation
    maxSizeBytes: maxSizeBytes
  }
}
