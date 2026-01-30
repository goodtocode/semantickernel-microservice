@description('Name of the local virtual network (parent)')
param localVnetName string

@description('Peering display name (local -> remote)')
param peeringName string

@description('Remote VNet full resource ID')
param remoteVnetId string

@description('Enable gateway transit (only if this is the gateway owner)')
param allowGatewayTransit bool = false

@description('Use remote gateways (only if the remote allows gateway transit)')
param useRemoteGateways bool = false

@description('Allow VNet access')
param allowVnetAccess bool = true

@description('Allow forwarded traffic')
param allowForwardedTraffic bool = false

resource localVnet 'Microsoft.Network/virtualNetworks@2024-09-01' existing = {
  name: localVnetName
}

resource localToRemote 'Microsoft.Network/virtualNetworks/virtualNetworkPeerings@2025-03-01' = {
  name: peeringName
  parent: localVnet
  properties: {
    remoteVirtualNetwork: { id: remoteVnetId }
    allowVirtualNetworkAccess: allowVnetAccess
    allowForwardedTraffic: allowForwardedTraffic
    allowGatewayTransit: allowGatewayTransit
    useRemoteGateways: useRemoteGateways
  }
}
