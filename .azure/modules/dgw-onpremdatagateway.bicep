
@description('The name of the On-premises Data Gateway. Must be 1-80 characters, using only alphanumeric characters and hyphens.')
@minLength(1)
@maxLength(80)
param name string

@description('The Azure region where the On-premises Data Gateway will be deployed.')
param location string = resourceGroup().location

@description('The installation ID for the On-premises Data Gateway.')
@minLength(1)
@maxLength(128)
param dgwInstallationId string

@description('The subscription ID for the On-premises Data Gateway. Defaults to the current subscription.')
param subscriptionId string = subscription().id

@description('Tags to apply to the On-premises Data Gateway resource.')
param tags object = {}

var locationShortName = toLower(replace(location, ' ', ''))
var gatewayInstallationId = '${subscriptionId}/providers/Microsoft.Web/locations/${locationShortName}/connectionGatewayInstallations/${dgwInstallationId}'

resource name_resource 'Microsoft.Web/connectionGateways@2016-06-01' = {
  name: name
  location: locationShortName
  tags: tags
  properties: {
    connectionGatewayInstallation: {
      id: gatewayInstallationId
    }
  }
}
