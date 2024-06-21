param name string
param tags object
param password string
param keyVaultId string
param keyVaultSecretName string
param serverFarmId string
param canonicalName string
param domainValidationMethod string
param hostnames array
param pfxBlob array

var location = resourceGroup().location

resource name_resource 'Microsoft.Web/certificates@2020-12-01' = {
  name: name
  location: location
  tags: tags
  properties: {
    hostNames: [
      hostnames
    ]
    pfxBlob: [
      pfxBlob
    ]
    password: password
    keyVaultId: keyVaultId
    keyVaultSecretName: keyVaultSecretName
    serverFarmId: serverFarmId
    canonicalName: canonicalName
    domainValidationMethod: domainValidationMethod
  }
}