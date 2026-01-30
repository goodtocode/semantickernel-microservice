@description('The name of the Static Web App. Must be 1-40 characters, using only alphanumeric characters and hyphens.')
@minLength(1)
@maxLength(40)
param name string

@description('The Azure region where the Static Web App will be deployed.')
param location string = resourceGroup().location

@description('The SKU (pricing tier) for the Static Web App. Allowed values: Free, Standard. Default is Free.')
@allowed([
  'Free'
  'Standard'
])
param sku string = 'Free'

@description('Tags to add to the Static Web App resource.')
param tags object = {}

@description('The Git repository URL for the Static Web App source code.')
@minLength(1)
param repositoryUrl string

@description('The Git branch to deploy from. Default is main.')
@minLength(1)
param branch string = 'main'

resource name_resource 'Microsoft.Web/staticSites@2022-09-01' = {
  name: name
  location: location
  tags: empty(tags) ? null : tags
  sku: {
    tier: sku
    name: sku
  }
  properties: {
    repositoryUrl: repositoryUrl
    branch: branch
  }
}

