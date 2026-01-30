
@description('The name of the Hybrid Connection. Must be 1-80 characters, using only alphanumeric characters and hyphens.')
@minLength(1)
@maxLength(80)
param name string

@description('The name of the Azure Relay namespace. Must be 6-50 characters, using only alphanumeric characters and hyphens.')
@minLength(6)
@maxLength(50)
param relayName string

resource relayName_name 'Microsoft.Relay/namespaces/hybridConnections@2021-11-01' = {
  name: '${relayName}/${name}'
  properties: {
    requiresClientAuthorization: true
  }
}
