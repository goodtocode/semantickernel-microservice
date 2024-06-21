param name string
param location string = resourceGroup().location
param subscriptionId string = subscription().subscriptionId
param dgwName string
param dgwResourceGroupName string

@allowed([
  'basic'
  'windows'
])
param sqlAuthType string = 'windows'
param sqlServerName string
param sqlDatabaseName string
param sqlUserName string
param sqlUserPassword string
param encryptConnection bool = false
param privacySetting string = 'None'

var locationShortName = toLower(replace(location, ' ', ''))

resource name_resource 'Microsoft.Web/connections@2018-07-01-preview' = {
  name: name
  location: locationShortName
  kind: 'V1'
  properties: {
    displayName: name
    customParameterValues: {}
    api: {
      id: '/subscriptions/${subscriptionId}/providers/Microsoft.Web/locations/${locationShortName}/managedApis/sql'
    }
    parameterValues: {
      server: sqlServerName
      database: sqlDatabaseName
      username: sqlUserName
      password: sqlUserPassword
      authType: sqlAuthType
      encryptConnection: encryptConnection
      privacySetting: privacySetting
      gateway: {
        id: '/subscriptions/${subscriptionId}/resourceGroups/${dgwResourceGroupName}/providers/Microsoft.Web/connectionGateways/${dgwName}'
      }
    }
  }
}