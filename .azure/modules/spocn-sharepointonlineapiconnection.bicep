
@description('The name of the SharePoint Online API Connection. Must be 1-80 characters, using only alphanumeric characters and hyphens.')
@minLength(1)
@maxLength(80)
param name string

@description('The Azure AD tenant ID for the SharePoint Online connection. Defaults to the current subscription tenant.')
param tenantId string = subscription().tenantId

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
      displayName: 'SharePoint'
      description: 'SharePoint helps organizations share and collaborate with colleagues, partners, and customers. You can connect to SharePoint Online or to an on-premises SharePoint 2013 or 2016 farm using the On-Premises Data Gateway to manage documents and list items.'
      iconUri: 'https://connectoricons-prod.azureedge.net/releases/v1.0.1533/1.0.1533.2600/${nameLower}/icon.png'
      brandColor: '#036C70'
      id: '${subscription().id}/providers/Microsoft.Web/locations/${locationShortName}/managedApis/sharepointonline'
      type: 'Microsoft.Web/locations/managedApis'
    }
    nonSecretParameterValues: {
      'token:TenantId': tenantId
    }
    parameterValues: {}
  }
  dependsOn: []
}
