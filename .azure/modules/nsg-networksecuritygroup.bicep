@description('Array of NSG security rules to apply to this NSG')
param securityRules array = []
@description('The name of the Network Security Group (NSG)')
param name string

@description('Tags to apply to the NSG')
param tags object = {}

@description('Location for the NSG')
param location string = resourceGroup().location

resource nsgResource 'Microsoft.Network/networkSecurityGroups@2023-05-01' = {
	name: name
	location: location
	tags: tags
}

resource rule 'Microsoft.Network/networkSecurityGroups/securityRules@2023-05-01' = [for ruleObj in securityRules: {
	name: ruleObj.name
	parent: nsgResource
	properties: {
		priority: ruleObj.priority
		direction: ruleObj.direction
		access: ruleObj.access
		protocol: ruleObj.protocol
		sourcePortRange: ruleObj.sourcePortRange
		destinationPortRange: ruleObj.destinationPortRange
		sourceAddressPrefix: ruleObj.sourceAddressPrefix
		destinationAddressPrefix: ruleObj.destinationAddressPrefix
	}
}]

output id string = nsgResource.id
