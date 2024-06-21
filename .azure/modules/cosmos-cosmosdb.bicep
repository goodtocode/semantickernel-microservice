@description('The DocumentDB database account name.')
@minLength(3)
param name string

@description('The DocumentDB default consistency level for this account.')
@allowed([
  'Eventual'
  'Strong'
  'Session'
  'BoundedStaleness'
])
param consistencyLevel string = 'Session'

@description('When consistencyLevel is set to BoundedStaleness, then this value is required, otherwise it can be ignored.')
@minValue(10)
@maxValue(1000)
param maxStalenessPrefix int = 10

@description('When consistencyLevel is set to BoundedStaleness, then this value is required, otherwise it can be ignored.')
@minValue(5)
@maxValue(600)
param maxIntervalInSeconds int = 5

var offerType = 'Standard'

resource name_resource 'Microsoft.DocumentDB/databaseAccounts@2015-04-08' = {
  name: name
  location: resourceGroup().location
  properties: {
    name: name
    databaseAccountOfferType: offerType
    consistencyPolicy: {
      defaultConsistencyLevel: consistencyLevel
      maxStalenessPrefix: maxStalenessPrefix
      maxIntervalInSeconds: maxIntervalInSeconds
    }
  }
}