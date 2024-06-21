param name string

@allowed([
  'F0'
  'S0'
])
param sku string = 'F0'

@allowed([
  'westus'
  'eastus'
])
param location string = 'westus'
param location2 string = resourceGroup().location
param location3 string = resourceGroup().location
param azureSearchLocation string = resourceGroup().location

@allowed([
  'free'
  'basic'
  'standard'
])
param azureSearchSku string = 'free'

@allowed([
  'Default'
])
param searchHostingMode string = 'Default'

@allowed([
  'F0'
  'S1'
])
param farmSku string = 'F0'

var puredAzureSearchName = replace(name, '-', '')
var normalizedAzureSearchName = ((length(puredAzureSearchName) > 40) ? substring(puredAzureSearchName, (length(puredAzureSearchName) - 40), 40) : puredAzureSearchName)
var azureSearchName_var = toLower('srch-${normalizedAzureSearchName}')
var appiName = 'appi-${name}'

resource name_resource 'Microsoft.CognitiveServices/accounts@2017-04-18' = {
  kind: 'QnAMaker'
  name: name
  location: location
  sku: {
    name: sku
  }
  properties: {
    apiProperties: {
      qnaRuntimeEndpoint: 'https://${Microsoft_Web_sites_name.properties.hostNames[0]}'
    }
    customSubDomainName: name
  }
  dependsOn: [
    azureSearchName
    resourceId('microsoft.insights/components/', appiName)
  ]
}

resource azureSearchName 'Microsoft.Search/searchServices@2015-08-19' = {
  name: azureSearchName_var
  location: azureSearchLocation
  tags: {}
  properties: {
    replicaCount: 1
    partitionCount: 1
    hostingMode: searchHostingMode
  }
  sku: {
    name: azureSearchSku
  }
}

resource Microsoft_Web_sites_name 'Microsoft.Web/sites@2016-08-01' = {
  name: name
  location: location3
  properties: {
    enabled: true
    siteConfig: {
      cors: {
        allowedOrigins: [
          '*'
        ]
      }
    }
    name: name
    serverFarmId: '/subscriptions/${subscription().subscriptionId}/resourcegroups/${resourceGroup().name}/providers/Microsoft.Web/serverfarms/${name}'
    hostingEnvironment: ''
  }
  tags: {
    'hidden-related:/subscriptions/${subscription().subscriptionId}/resourcegroups/${resourceGroup().name}/providers/Microsoft.Web/serverfarms/${name}': 'empty'
  }
  dependsOn: [
    Microsoft_Web_serverfarms_name
  ]
}

resource name_appiName 'Microsoft.Web/sites/microsoft.insights/components@2015-05-01' = {
  name: '${name}/${appiName}'
  kind: 'web'
  location: location2
  tags: {
    'hidden-link:${Microsoft_Web_sites_name.id}': 'Resource'
  }
  properties: {
    ApplicationId: name
  }
}

resource name_appsettings 'Microsoft.Web/sites/config@2021-06-01' = {
  parent: Microsoft_Web_sites_name
  name: 'appsettings'
  properties: {
    AzureSearchName: azureSearchName_var
    AzureSearchAdminKey: listAdminKeys(azureSearchName.id, '2015-08-19').primaryKey
    UserappiKey: reference(resourceId('microsoft.insights/components/', appiName), '2015-05-01').InstrumentationKey
    UserappiName: appiName
    UserAppInsightsAppId: reference(resourceId('microsoft.insights/components/', appiName), '2015-05-01').AppId
    PrimaryEndpointKey: '${name}-PrimaryEndpointKey'
    SecondaryEndpointKey: '${name}-SecondaryEndpointKey'
    DefaultAnswer: 'No good match found in KB.'
    QNAMAKER_EXTENSION_VERSION: 'latest'
  }
}

resource Microsoft_Web_serverfarms_name 'Microsoft.Web/serverfarms@2016-09-01' = {
  name: name
  location: location3
  properties: {
    name: name
    workerSizeId: '0'
    reserved: false
    numberOfWorkers: '1'
    hostingEnvironment: ''
  }
  sku: {
    name: farmSku
  }
}

output qnaRuntimeEndpoint string = 'https://${Microsoft_Web_sites_name.properties.hostNames[0]}'