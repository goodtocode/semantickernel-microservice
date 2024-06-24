param name string
param location string
param sku string
param tags object

resource workResource 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: name
  location: location
  tags: tags
  properties: {
    sku: {
      name: sku
    }
  }
}

output id string  = workResource.id
