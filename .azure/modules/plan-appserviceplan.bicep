
param name string 
param location string 
param sku string 
param tags object

resource planResource 'Microsoft.Web/serverfarms@2023-12-01' = {
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
