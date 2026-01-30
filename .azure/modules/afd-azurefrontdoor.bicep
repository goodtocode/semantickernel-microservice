

@description('Name of the Azure Front Door instance. Must be globally unique and 3-63 characters, using only lowercase letters, numbers, and hyphens, starting and ending with a letter or number.')
@minLength(3)
@maxLength(63)
param name string

@description('Location for the Azure Front Door resource. Must be set to global.')
@allowed(['global'])
param location string = 'global'

@description('Tags to apply to the Azure Front Door resource.')
param tags object = {}

@description('SKU for Azure Front Door. Allowed values: Standard_AzureFrontDoor, Premium_AzureFrontDoor. Default is Standard_AzureFrontDoor.')
@allowed([
	'Standard_AzureFrontDoor'
	'Premium_AzureFrontDoor'
])
param sku string = 'Standard_AzureFrontDoor'

resource afd 'Microsoft.Cdn/profiles@2023-05-01' = {
	name: name
	location: location
	tags: tags
	sku: {
		name: sku
	}
}

output id string = afd.id
