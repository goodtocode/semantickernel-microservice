@description('Specifies the name of the App Configuration store. 5-50 chars, lowercase letters, numbers, and -')
@minLength(5)
@maxLength(50)
param name string

@description('Specifies the sku of the App Configuration store.')
@allowed([
  'free'
  'standard'
])
param sku string = 'free'

@description('Specifies the Azure location where the app configuration store should be created.')
param location string = toLower(replace(resourceGroup().location, ' ', ''))

resource name_resource 'Microsoft.AppConfiguration/configurationStores@2023-03-01' = {
  name: name
  location: location
  sku: {
    name: sku
  }
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    disableLocalAuth: true
    publicNetworkAccess: 'Disabled'
  }
}


