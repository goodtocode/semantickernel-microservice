@minLength(1)
@maxLength(60)
param name string

@minLength(1)
@maxLength(60)
param adminLogin string

@minLength(1)
@maxLength(128)
@secure()
param adminPassword string

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
param location string = toLower(replace(resourceGroup().location, ' ', ''))
param maxSizeBytes int = 1073741824

@description('Describes the performance level for Sku')
@allowed([
  'Basic'
  'S0'
  'S1'
  'S2'
  'P1'
  'P2'
  'P3'
])
param skuPerformanceLevel string = 'Basic'

resource sqlServer 'Microsoft.Sql/servers@2021-11-01' = {
  name: name
  location: location
  tags: {
    displayName: 'SqlServer'
  }
  properties: {
    administratorLogin: adminLogin
    administratorLoginPassword: adminPassword
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-05-01-preview' = {
  parent: sqlServer
  name: sqldbName
  location: location
  tags: {
    displayName: 'Database'
  }
  sku: {
    name: sku
    //tier: 'skuTier' // Replace with the desired SKU tier (e.g., Basic, GeneralPurpose, BusinessCritical)
    //family: 'skuFamily' // Replace with the desired SKU family (e.g., Gen4, Gen5)
    capacity: 1 // Replace with the desired capacity (e.g., 1, 2, 4)
  }
  properties: {
    collation: 'collation'
    maxSizeBytes: maxSizeBytes
  }
}

// resource sqlDatabase 'Microsoft.Sql/servers/databases@2021-11-01' = {
//   parent: sqlServer
//   name: sqldbName
//   location: location
//   tags: {
//     displayName: 'Database'
//   }
//   properties: {
//     edition: sku
//     collation: collation
//     maxSizeBytes: maxSizeBytes
//     skuPerformanceLevel: skuPerformanceLevel
//   }
// }

resource sqlFirewall 'Microsoft.Sql/servers/firewallRules@2021-11-01' = {
  parent: sqlServer
  location: resourceGroup().location
  name: 'AllowAllWindowsAzureIps'
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}
