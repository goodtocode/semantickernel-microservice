@minLength(1)
@maxLength(60)
param name string
param location string = resourceGroup().location
param tags object
@description('Sku for the database')
@allowed([
  'Basic'
  'Standard'
  'Premium'
])
param sku string = 'Basic'
param collation string = 'SQL_Latin1_General_CP1_CI_AS'
param maxSizeBytes int = 1073741824
param sqlName string

resource sqlServer 'Microsoft.Sql/servers@2023-08-01-preview' existing = {
  name: sqlName 
  //scope: resourceGroup(sqlSubscriptionId, sqlResourceGroupName)
}
// param sqlSubscriptionId string = subscription().subscriptionId
// param sqlResourceGroupName string = resourceGroup().name
//name: '${sqlName}/${name}'
resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-08-01-preview' = {
  name: name
  parent: sqlServer
  location: location
  tags: tags
  //parent: sqlServerResource
  sku: {
    name: sku
    tier: sku // Replace with the desired SKU tier (e.g., Basic, GeneralPurpose, BusinessCritical)
    //family: 'skuFamily' // Replace with the desired SKU family (e.g., Gen4, Gen5)
    capacity: 1 // Replace with the desired capacity (e.g., 1, 2, 4)
  }
  properties: {
    collation: collation
    maxSizeBytes: maxSizeBytes
  }
}
