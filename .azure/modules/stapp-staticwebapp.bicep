//https://www.aaron-powell.com/posts/2022-06-29-deploy-swa-with-bicep/
//https://stackoverflow.com/questions/77002723/deploy-blazor-wasm-with-net-8-using-github-actions
//https://azure.github.io/static-web-apps-cli/docs/cli/swa-deploy/
@description('Name of the Static Web App. (stapp)')
param name string

@description('Azure region of the deployment')
param location string = resourceGroup().location

@allowed([ 'Free', 'Standard' ])
param sku string = 'Free'

@description('Tags to add to the resources')
param tags object = {}

resource name_resource 'Microsoft.Web/staticSites@2022-09-01' = {
  name: name
  location: location
  tags: tags
  sku: {
    tier: sku
    name: sku
  }
}
