
@description('The name of the API Management service instance. Must be 1-50 characters, use only letters, numbers, and hyphens, and start/end with a letter or number.')
@minLength(1)
@maxLength(50)
param name string = 'apiservice${uniqueString(resourceGroup().id)}'

@description('The email address of the owner of the API Management service. Must be a valid email address.')
@minLength(5)
@maxLength(100)
param publisherEmail string

@description('The name of the owner of the API Management service. Must be at least 1 character.')
@minLength(1)
param publisherName string

@description('The pricing tier (SKU) of this API Management service. Allowed values: Developer, Standard, Premium. Default is Developer.')
@allowed([
  'Developer'
  'Standard'
  'Premium'
])
param sku string = 'Developer'

@description('Tags to apply to the API Management service resource.')
param tags object = {}

@description('The instance size (capacity) of this API Management service. Allowed values: 1, 2. Default is 1.')
@allowed([
  1
  2
])
param capacity int = 1

@description('Location for all resources. Defaults to the resource group location.')
param location string = resourceGroup().location

resource apimResource 'Microsoft.ApiManagement/service@2023-05-01-preview' = {
  name: name
  location: location
  tags: tags
  sku: {
    name: sku
    capacity: capacity
  }
  properties: {
    publisherEmail: publisherEmail
    publisherName: publisherName
  }
}
