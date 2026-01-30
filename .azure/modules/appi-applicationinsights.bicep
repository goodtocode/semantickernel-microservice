

@description('The Azure region where the Application Insights resource will be deployed. Allowed: eastus, eastus2, westus, westus2, centralus.')
@allowed([
  'eastus'
  'eastus2'
  'westus'
  'westus2'
  'centralus'
])
param location string

@description('Tags to apply to the Application Insights resource.')
param tags object = {}

@description('Specifies the name of the Application Insights resource. 1-255 characters, letters, numbers, and -')
@minLength(1)
@maxLength(255)
param name string

@description('The resource ID of the Log Analytics workspace to link to Application Insights. Must be a valid resourceId string.')
@minLength(1)
param workResourceId string

resource appiResource 'Microsoft.Insights/components@2020-02-02' = {
  name: name
  location: location
  tags: empty(tags) ? null : tags
  kind:'web'
  properties: {
    Application_Type: 'web'
    Flow_Type: 'Bluefield'
    WorkspaceResourceId: workResourceId
  }
}

output id string = appiResource.id
output InstrumentationKey string  = appiResource.properties.InstrumentationKey
output Connectionstring string = appiResource.properties.ConnectionString
