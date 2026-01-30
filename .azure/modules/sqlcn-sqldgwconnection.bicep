
@description('The name of the SQL Data Gateway Connection. Must be 1-80 characters, using only alphanumeric characters and hyphens.')
@minLength(1)
@maxLength(80)
param name string

@description('The Azure region where the SQL Data Gateway Connection will be deployed.')
param location string = resourceGroup().location

@description('The subscription ID for the connection. Defaults to the current subscription.')
param subscriptionId string = subscription().subscriptionId

@description('The name of the Data Gateway.')
@minLength(1)
@maxLength(80)
param dgwName string

@description('The resource group name of the Data Gateway.')
@minLength(1)
@maxLength(90)
param dgwResourceGroupName string


@description('The SQL authentication type. Allowed values: basic, windows. Default is windows.')
@allowed([
  'basic'
  'windows'
])
param sqlAuthType string = 'windows'

@description('The name of the SQL Server.')
@minLength(1)
@maxLength(128)
param sqlServerName string

@description('The name of the SQL Database.')
@minLength(1)
@maxLength(128)
param sqlDatabaseName string

@description('The SQL user name.')
@minLength(1)
@maxLength(128)
param sqlUserName string

@description('The SQL user password.')
@minLength(1)
@maxLength(128)
@secure()
param sqlUserPassword string

@description('Whether to encrypt the SQL connection. Default is false.')
param encryptConnection bool = false

@description('The privacy setting for the connection. Default is None.')
param privacySetting string = 'None'

var locationShortName = toLower(replace(location, ' ', ''))

resource conection 'Microsoft.Web/connections@2016-06-01' = {
  name: name
  location: locationShortName
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
