param workName string

resource workName_resource 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: workName
  location: resourceGroup().location
  properties: {}
}
