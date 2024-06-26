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

resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-08-01-preview' = {
  name: '${sqlName}/${name}'
  location: location
  tags: tags
  sku: {
    name: sku
    tier: sku // (e.g., Basic, GeneralPurpose, BusinessCritical)
    //family: 'skuFamily' // (e.g., Gen4, Gen5)
    capacity: 1 // (e.g., 1, 2, 4)
  }
  properties: {
    collation: collation
    maxSizeBytes: maxSizeBytes
  }
}
