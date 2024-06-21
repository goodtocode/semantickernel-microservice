@minLength(1)
@maxLength(60)
param name string

@minLength(1)
@maxLength(60)
param planName string

resource name_resource 'Microsoft.Web/sites@2018-11-01' = {
  name: name
  location: resourceGroup().location
  tags: {
    'hidden-related:${resourceGroup().id}/providers/Microsoft.Web/serverfarms/${planName}': 'Resource'
    displayName: 'Website'
  }
  properties: {
    name: name
    serverFarmId: resourceId('Microsoft.Web/serverfarms', planName)
  }
  dependsOn: []
}