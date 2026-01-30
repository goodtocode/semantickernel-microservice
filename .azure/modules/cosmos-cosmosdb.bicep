
@description('The name of the Cosmos DB account. Must be 3-44 characters, using only lowercase letters, numbers, and hyphens.')
@minLength(3)
@maxLength(44)
param name string

@description('The default consistency level for the Cosmos DB account. Allowed values: Eventual, Strong, Session, BoundedStaleness. Default is Session.')
@allowed([
  'Eventual'
  'Strong'
  'Session'
  'BoundedStaleness'
])
param consistencyLevel string = 'Session'

@description('The max staleness prefix for BoundedStaleness consistency. Required if consistencyLevel is BoundedStaleness. Default is 10.')
@minValue(10)
@maxValue(1000)
param maxStalenessPrefix int = 10

@description('The max interval in seconds for BoundedStaleness consistency. Required if consistencyLevel is BoundedStaleness. Default is 5.')
@minValue(5)
@maxValue(600)
param maxIntervalInSeconds int = 5

var offerType = 'Standard'

resource name_resource 'Microsoft.DocumentDB/databaseAccounts@2024-05-15' = {
  name: name
  location: resourceGroup().location
  properties: {
    databaseAccountOfferType: offerType
    consistencyPolicy: {
      defaultConsistencyLevel: consistencyLevel
      maxStalenessPrefix: maxStalenessPrefix
      maxIntervalInSeconds: maxIntervalInSeconds
    }
    locations: [
      {
        locationName: resourceGroup().location
        failoverPriority: 0
        isZoneRedundant: false
      }
    ]
  }
}
