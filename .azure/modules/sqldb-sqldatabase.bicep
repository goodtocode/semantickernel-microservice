
@description('The name of the SQL Database. Must be 1-60 characters, using only alphanumeric characters and hyphens.')
@minLength(1)
@maxLength(60)
param name string

@description('The Azure region where the SQL Database will be deployed.')
param location string = resourceGroup().location

@description('Tags to apply to the SQL Database resource.')
param tags object = {}

@description('The SKU (pricing tier) for the SQL Database. Allowed values: Basic, Standard, Premium. Default is Basic.')
@allowed([
  'Basic'
  'Standard'
  'Premium'
])
param sku string = 'Basic'

@description('The compute capacity for the SQL Database SKU. Default is 5.')
@minValue(1)
param sqlCapacity int = 5

@description('The collation for the SQL Database. Default is SQL_Latin1_General_CP1_CI_AS.')
param collation string = 'SQL_Latin1_General_CP1_CI_AS'

@description('The maximum size of the SQL Database in bytes. Default is 1073741824 (1 GB).')
@minValue(1048576)
param maxSizeBytes int = 1073741824

@description('The name of the parent SQL Server resource.')
@minLength(1)
@maxLength(60)
param sqlName string

resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-08-01-preview' = {
  name: '${sqlName}/${name}'
  location: location
  tags: empty(tags) ? null : tags
  sku: {
    name: sku
    tier: sku // (e.g., Basic, GeneralPurpose, BusinessCritical)
    //family: 'skuFamily' // (e.g., Gen4, Gen5)
    capacity: sqlCapacity // (e.g., 1, 2, 4)
  }
  properties: {
    collation: collation
    maxSizeBytes: maxSizeBytes
  }
}
