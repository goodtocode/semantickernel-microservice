
@description('The name of the Teams API Connection. Must be 1-80 characters, using only alphanumeric characters and hyphens.')
@minLength(1)
@maxLength(80)
param name string

var locationShortName = toLower(replace(resourceGroup().location, ' ', ''))
var nameLower = toLower(replace(replace(name, '-', ''), ' ', ''))

resource connection 'Microsoft.Web/connections@2016-06-01' = {
  name: nameLower
  location: locationShortName
  properties: {
    displayName: name
    customParameterValues: {}
    api: {
      name: nameLower
      displayName: 'Microsoft Teams'
      description: 'Microsoft Teams enables you to get all your content, tools and conversations in the Team workspace with Office 365.'
      iconUri: 'https://connectoricons-prod.azureedge.net/releases/v1.0.1505/1.0.1505.2520/${nameLower}/icon.png'
      brandColor: '#4B53BC'
      id: '/subscriptions/${subscription().subscriptionId}/providers/Microsoft.Web/locations/${locationShortName}/managedApis/teams'
      type: 'Microsoft.Web/locations/managedApis'
    }
    parameterValues: {}
  }
  dependsOn: []
}
