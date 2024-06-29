@minLength(1)
param name string

@description('Describes plan\'s pricing tier and capacity. Check details at https://azure.microsoft.com/en-us/pricing/details/app-service/')
@allowed([
  'F1'
  'D1'
  'B1'
  'B2'
  'B3'
  'S1'
  'S2'
  'S3'
  'P1'
  'P2'
  'P3'
  'P4'
  'Y1'
])
param sku string = 'F1'

@description('Describes plan\'s instance count')
@minValue(1)
param skuCapacity int = 1

resource name_resource 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: name
  location: resourceGroup().location
  tags: {
    displayName: 'HostingPlan'
  }
  sku: {
    name: sku
    capacity: skuCapacity
  }
  properties: {
    name: name
  }
}
