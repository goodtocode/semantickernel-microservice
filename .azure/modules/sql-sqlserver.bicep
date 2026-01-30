
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

@description('The starting IP address for the SQL Server firewall rule. Default is 0.0.0.0.')
param startIpAddress string = '0.0.0.0'

@description('The ending IP address for the SQL Server firewall rule. Default is 0.0.0.0.')
param endIpAddress string = '0.0.0.0'


var nameLower = toLower(name)

resource sqlServer 'Microsoft.Sql/servers@2023-08-01-preview' = {
  name: nameLower
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

output id string  = sqlServer.id
output name string  = sqlServer.name
