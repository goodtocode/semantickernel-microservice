@description('The name of the App Service Plan. Must be 1-40 characters, using only alphanumeric characters and hyphens.')
@minLength(1)
@maxLength(40)
param name string

@description('The Azure region where the App Service Plan will be deployed.')
param location string

@description('The SKU (pricing tier) for the App Service Plan. Allowed values: F1, D1, B1, B2, B3, S1, S2, S3, P1, P2, P3, P4, Y1. Default is F1.')
@allowed([
  'F1'
  'D1'
  'B1'
  'B2'
  'B3'
  'S1'
  'S2'
  'S3'
  'P1'
  'P2'
  'P3'
  'P4'
  'Y1'
])
param sku string = 'F1'

@description('Tags to apply to the App Service Plan resource.')
param tags object = {}

@description('The OS type for the App Service Plan. Allowed values: Windows, Linux.')
@allowed([
  'Windows'
  'Linux'
])
param osType string = 'Windows'

@description('Enable zone redundancy for the App Service Plan (PremiumV2 and higher only).')
param zoneRedundant bool = false

@description('The number of worker instances.')
@minValue(1)
param capacity int = 1

@description('Enable per-site scaling (dedicated plans only).')
param perSiteScaling bool = false

@description('Enable elastic scale (Premium plans only).')
param elasticScaleEnabled bool = false

@description('Indicates if the plan is reserved for Linux (true) or Windows (false).')
param reserved bool = (osType == 'Linux')

@description('Enable diagnostics settings for the App Service Plan.')
param enableDiagnostics bool = false

@description('Diagnostics settings configuration (if enabled).')
param diagnosticsSettings object = {}

resource planResource 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: name
  location: location
  kind: osType
  tags: empty(tags) ? null : tags
  sku: {
    name: sku
    capacity: capacity
  }
  properties: {
    reserved: reserved
    perSiteScaling: perSiteScaling
    elasticScaleEnabled: elasticScaleEnabled
    zoneRedundant: zoneRedundant
  }
}

resource diag 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = if (enableDiagnostics) {
  name: '${planResource.name}-diagnostics'
  scope: planResource
  properties: diagnosticsSettings
}

output id string = planResource.id
output name string = planResource.name
output location string = planResource.location
output kind string = planResource.kind
output sku object = planResource.sku
output properties object = planResource.properties
