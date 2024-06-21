param name string
param tenantId string = subscription().tenantId

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
      displayName: 'SharePoint'
      description: 'SharePoint helps organizations share and collaborate with colleagues, partners, and customers. You can connect to SharePoint Online or to an on-premises SharePoint 2013 or 2016 farm using the On-Premises Data Gateway to manage documents and list items.'
      iconUri: 'https://connectoricons-prod.azureedge.net/releases/v1.0.1533/1.0.1533.2600/${nameLower_var}/icon.png'
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