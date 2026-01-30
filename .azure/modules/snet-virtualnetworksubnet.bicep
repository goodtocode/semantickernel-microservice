@description('Name of the existing virtual network to add the subnet to')
param vnetName string

@description('Name of the subnet to create')
param snetName string

@description('Address prefix for the subnet (e.g., 10.0.0.0/24)')
param cidr string

@description('Resource ID of the Network Security Group to associate with the subnet')
param nsgId string = ''

resource subnet 'Microsoft.Network/virtualNetworks/subnets@2025-01-01' = {
	name: '${vnetName}/${snetName}'
	properties: {
		addressPrefix: cidr
		networkSecurityGroup: empty(nsgId) ? null : {
			id: nsgId
		}
	}
}

output id string = subnet.id
