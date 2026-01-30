targetScope = 'resourceGroup'

param location string = resourceGroup().location
param tags object
param vnetName string
param vnetCidr string
param snetNameManagement string
param snetCidrManagement string

var platformManagementSecurityRules = [
  {
    name: 'AllowPlatformHTTP'
    priority: 100
    direction: 'Inbound'
    access: 'Allow'
    protocol: 'Tcp'
    sourcePortRange: '*'
    destinationPortRange: '80'
    sourceAddressPrefix: 'VirtualNetwork'
    destinationAddressPrefix: '*'
  }
  {
    name: 'AllowPlatformHTTPS'
    priority: 110
    direction: 'Inbound'
    access: 'Allow'
    protocol: 'Tcp'
    sourcePortRange: '*'
    destinationPortRange: '443'
    sourceAddressPrefix: 'VirtualNetwork'
    destinationAddressPrefix: '*'
  }
  {
    name: 'DenyAllInbound'
    priority: 4096
    direction: 'Inbound'
    access: 'Deny'
    protocol: '*'
    sourcePortRange: '*'
    destinationPortRange: '*'
    sourceAddressPrefix: '*'
    destinationAddressPrefix: '*'
  }
]

module vnet '../modules/vnet-virtualnetwork.bicep' = {
  name: 'vnetName'
  params: {
    name: vnetName
    addressPrefix: vnetCidr
    location: location
    tags: tags
  }
}

module nsgSnet '../modules/nsg-networksecuritygroup.bicep' = {
  name: 'nsgNameManagement'
  params: {
    name: '${snetNameManagement}-nsg'
    tags: tags
    securityRules: platformManagementSecurityRules
  }
}

module snetHub '../modules/snet-virtualnetworksubnet.bicep' = {
  name: 'snetNameManagement'
  params: {
    vnetName: vnetName
    snetName: snetNameManagement
    cidr: snetCidrManagement
    nsgId: nsgSnet.outputs.id
  }
}
