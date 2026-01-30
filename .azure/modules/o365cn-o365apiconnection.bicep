
@description('The name of the Office 365 API Connection. Must be 1-80 characters, using only alphanumeric characters and hyphens. Default is teams.')
@minLength(1)
@maxLength(80)
param name string = 'teams'

var locationLower = toLower(replace(resourceGroup().location, ' ', ''))
var nameLower = toLower(replace(replace(name, '-', ''), ' ', ''))

resource connection 'Microsoft.Web/connections@2016-06-01' = {
  name: nameLower
  location: locationLower
  properties: {
    displayName: name
    customParameterValues: {}
    api: {
      name: nameLower
      displayName: 'Office 365'
      description: 'Microsoft Teams enables you to get all your content, tools and conversations in the Team workspace with Office 365.'
      iconUri: 'https://connectoricons-prod.azureedge.net/releases/v1.0.1505/1.0.1505.2520/${nameLower}/icon.png'
      brandColor: '#4B53BC'
      id: '${subscription().id}/providers/Microsoft.Web/locations/${resourceGroup().location}/managedApis/office365'
      type: 'Microsoft.Web/locations/managedApis'
    }
    parameterValues: {}
  }
  dependsOn: []
}
