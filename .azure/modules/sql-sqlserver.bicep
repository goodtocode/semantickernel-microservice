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

var nameLower_var = toLower(name)

resource nameLower 'Microsoft.Sql/servers@2014-04-01-preview' = {
  name: nameLower_var
  location: resourceGroup().location
  tags: {
    displayName: 'SqlServer'
  }
  properties: {
    adminLogin: adminLogin
    adminPassword: adminPassword
  }
}

resource nameLower_AllowAllWindowsAzureIps 'Microsoft.Sql/servers/firewallrules@2014-04-01-preview' = {
  parent: nameLower
  location: resourceGroup().location
  name: 'AllowAllWindowsAzureIps'
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}