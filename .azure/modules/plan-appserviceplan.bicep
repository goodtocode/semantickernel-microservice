@minLength(1)
param name string 

param location string 
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
param tags object

resource planResource 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: name
  kind:'Windows'
  location: location
  tags: tags
  properties: {
    reserved: false    
  }
  sku: {
    name: sku
  }
 
}

output id string = planResource.id
