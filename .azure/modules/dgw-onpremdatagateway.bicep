param name string
param location string = resourceGroup().location
param dgwInstallationId string
param subscriptionId string = subscription().id
param tagsByResource object

var locationShortName = toLower(replace(location, ' ', ''))
var gatewayInstallationId = '${subscriptionId}/providers/Microsoft.Web/locations/${locationShortName}/connectionGatewayInstallations/${dgwInstallationId}'

resource name_resource 'Microsoft.Web/connectionGateways@2016-06-01' = {
  name: name
  location: locationShortName
  tags: (contains(tagsByResource, 'Microsoft.Web/connectionGateways') ? tagsByResource.Microsoft.Web / connectionGateways : json('{}'))
  properties: {
    connectionGatewayInstallation: {
      id: gatewayInstallationId
    }
  }
}