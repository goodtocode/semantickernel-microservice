param name string
param location string
param sku string
param tags object = {}

resource workResource 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: name
  location: location
  tags: empty(tags) ? null : tags
  properties: {
    sku: {
      name: sku
    }
    retentionInDays: 30
  }
}

output id string  = workResource.id
