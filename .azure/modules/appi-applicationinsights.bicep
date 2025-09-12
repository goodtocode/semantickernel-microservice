
param location string 
param tags object = {}
param name string 
param Application_Type string 
param Flow_Type string 
param workResourceId string

resource appiResource 'Microsoft.Insights/components@2020-02-02' = {
  name: name
  location: location
  tags: empty(tags) ? null : tags
  kind:'web'
  properties: {
    Application_Type: Application_Type
    Flow_Type: Flow_Type
    WorkspaceResourceId: workResourceId
    
  }
}

output id string = appiResource.id
output InstrumentationKey string  = appiResource.properties.InstrumentationKey
output Connectionstring string = appiResource.properties.ConnectionString
