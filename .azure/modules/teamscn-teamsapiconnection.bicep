param name string

var locationShortName = toLower(replace(resourceGroup().location, ' ', ''))
var nameLower_var = toLower(replace(replace(name, '-', ''), ' ', ''))

resource nameLower 'Microsoft.Web/connections@2018-07-01-preview' = {
  name: nameLower_var
  location: locationShortName
  kind: 'V1'
  properties: {
    displayName: name
    customParameterValues: {}
    api: {
      name: nameLower_var
      displayName: 'Microsoft Teams'
      description: 'Microsoft Teams enables you to get all your content, tools and conversations in the Team workspace with Office 365.'
      iconUri: 'https://connectoricons-prod.azureedge.net/releases/v1.0.1505/1.0.1505.2520/${nameLower_var}/icon.png'
      brandColor: '#4B53BC'
      id: '/subscriptions/${subscription().subscriptionId}/providers/Microsoft.Web/locations/${locationShortName}/managedApis/teams'
      type: 'Microsoft.Web/locations/managedApis'
    }
    parameterValues: {}
  }
  dependsOn: []
}