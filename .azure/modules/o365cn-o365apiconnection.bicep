param name string = 'teams'

var locationLower = toLower(replace(resourceGroup().location, ' ', ''))
var nameLower_var = toLower(replace(replace(name, '-', ''), ' ', ''))

resource nameLower 'Microsoft.Web/connections@2021-06-01' = {
  name: nameLower_var
  location: locationLower
  kind: 'V1'
  properties: {
    displayName: name
    customParameterValues: {}
    api: {
      name: nameLower_var
      displayName: 'Office 365'
      description: 'Microsoft Teams enables you to get all your content, tools and conversations in the Team workspace with Office 365.'
      iconUri: 'https://connectoricons-prod.azureedge.net/releases/v1.0.1505/1.0.1505.2520/${nameLower_var}/icon.png'
      brandColor: '#4B53BC'
      id: '${subscription().id}/providers/Microsoft.Web/locations/${resourceGroup().location}/managedApis/office365'
      type: 'Microsoft.Web/locations/managedApis'
    }
    parameterValues: {}
  }
  dependsOn: []
}