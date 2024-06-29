param name string
param relayName string
param location string = resourceGroup().location

resource relayName_name 'Microsoft.Relay/namespaces/hybridConnections@2021-11-01' = {
  name: '${relayName}/${name}'
  location: location
  properties: {
    requiresClientAuthorization: true
  }
}
