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
param databaseName string
param collation string = 'SQL_Latin1_General_CP1_CI_AS'

@allowed([
  'Basic'
  'Standard'
  'Premium'
])
param sku string = 'Basic'
param location string = toLower(replace(resourceGroup().location, ' ', ''))
param maxSizeBytes string = '1073741824'

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
param requestedServiceObjectiveName string = 'Basic'

var serverNameLower_var = toLower(name)

resource serverNameLower 'Microsoft.Sql/servers@2014-04-01-preview' = {
  name: serverNameLower_var
  location: location
  tags: {
    displayName: 'SqlServer'
  }
  properties: {
    administratorLogin: adminLogin
    administratorLoginPassword: adminPassword
  }
}

resource serverNameLower_databaseName 'Microsoft.Sql/servers/databases@2014-04-01-preview' = {
  parent: serverNameLower
  name: '${databaseName}'
  location: location
  tags: {
    displayName: 'Database'
  }
  properties: {
    edition: sku
    collation: collation
    maxSizeBytes: maxSizeBytes
    requestedServiceObjectiveName: requestedServiceObjectiveName
  }
}

resource serverNameLower_AllowAllWindowsAzureIps 'Microsoft.Sql/servers/firewallrules@2014-04-01-preview' = {
  parent: serverNameLower
  location: resourceGroup().location
  name: 'AllowAllWindowsAzureIps'
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}