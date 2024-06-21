param name string
param relayName string
param location string = resourceGroup().location

resource relayName_name 'Microsoft.Relay/namespaces/HybridConnections@2017-04-01' = {
  name: '${relayName}/${name}'
  location: location
  properties: {
    requiresClientAuthorization: true
  }
}