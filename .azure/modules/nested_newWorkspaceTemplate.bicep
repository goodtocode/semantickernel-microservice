param workName string

resource workName_resource 'Microsoft.OperationalInsights/workspaces@2020-08-01' = {
  name: workName
  location: resourceGroup().location
  properties: {}
}