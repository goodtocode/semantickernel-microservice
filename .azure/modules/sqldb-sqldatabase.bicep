@minLength(1)
@maxLength(60)
param name string

param location string = resourceGroup().location

param tags object

param collation string = 'SQL_Latin1_General_CP1_CI_AS'

param maxSizeBytes int = 1073741824
@description('Sku for the database')
@allowed([
  'Basic'
  'Standard'
  'Premium'
])
param sku string = 'Basic'

param sqlResourceId string

var sqldbResourceId = '${sqlResourceId}/databases/${name}'

resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-08-01-preview' = {
  name: sqldbResourceId
  location: location
  tags: tags
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
