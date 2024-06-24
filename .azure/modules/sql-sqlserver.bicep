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


var nameLower = toLower(name)

resource sqlServer 'Microsoft.Sql/servers@2023-08-01-preview' = {
  name: nameLower
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

output id string  = sqlServer.id
output name string  = sqlServer.name
