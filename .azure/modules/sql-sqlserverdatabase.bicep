
// Sql Server
@description('The name of the SQL Server. Must be 1-60 characters, using only alphanumeric characters and hyphens.')
@minLength(1)
@maxLength(60)
param name string

@description('The Azure region where the SQL Server will be deployed.')
param location string = resourceGroup().location

@description('Tags to apply to the SQL Server resource.')
param tags object = {}

@description('The administrator login name for the SQL Server. Must be 1-60 characters.')
@minLength(1)
@maxLength(60)
param adminLogin string

@description('The administrator password for the SQL Server. Must be 1-128 characters.')
@minLength(1)
@maxLength(128)
@secure()
param adminPassword string

@description('The starting IP address for the SQL Server firewall rule.')
param startIpAddress string = '0.0.0.0'

@description('The ending IP address for the SQL Server firewall rule.')
param endIpAddress string = '0.0.0.0'

// Sql Database
@description('The name of the SQL Database. Must be 1-60 characters, using only alphanumeric characters and hyphens.')
@minLength(1)
@maxLength(60)
param sqldbName string

@description('The compute capacity for the SQL Database SKU. Default is 5.')
@minValue(1)
param sqlCapacity int = 5

@description('The collation for the SQL Database. Default is SQL_Latin1_General_CP1_CI_AS.')
param collation string = 'SQL_Latin1_General_CP1_CI_AS'

@description('The SKU (pricing tier) for the SQL Database. Allowed values: Basic, Standard, Premium. Default is Basic.')
@allowed([
  'Basic'
  'Standard'
  'Premium'
])
param sku string = 'Basic'

@description('The maximum size of the SQL Database in bytes. Default is 1073741824 (1 GB).')
@minValue(1048576)
param maxSizeBytes int = 1073741824

resource sqlServer 'Microsoft.Sql/servers@2023-08-01-preview' = {
  name: name
  location: location
  tags: empty(tags) ? null : tags
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
    capacity: sqlCapacity // Replace with the desired capacity (e.g., 1, 2, 4)
  }
  properties: {
    collation: collation
    maxSizeBytes: maxSizeBytes
  }
}
