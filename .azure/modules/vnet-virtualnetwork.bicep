
@description('The name of the Virtual Network. Must be 2-64 characters, using only alphanumeric characters and hyphens.')
@minLength(2)
@maxLength(64)
param name string

@description('The address prefix (CIDR block) for the Virtual Network. Example: 10.0.0.0/16')
@minLength(9)
@maxLength(18)
param addressPrefix string

@description('The Azure region where the Virtual Network will be deployed.')
param location string

@description('Tags to apply to the Virtual Network resource.')
param tags object = {}

resource vnetResource 'Microsoft.Network/virtualNetworks@2023-02-01' = {
	name: name
	location: location
	tags: empty(tags) ? null : tags
	properties: {
		addressSpace: {
			addressPrefixes: [ addressPrefix ]
		}
		enableDdosProtection: false
		enableVmProtection: false
	}
}

output id string = vnetResource.id
