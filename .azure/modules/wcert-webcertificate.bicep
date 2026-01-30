
@description('The name of the Web Certificate resource. Must be 1-80 characters.')
@minLength(1)
@maxLength(80)
param name string

@description('Tags to apply to the Web Certificate resource.')
param tags object = {}

@description('The password for the PFX certificate. Must be at least 8 characters.')
@minLength(8)
@secure()
param password string

@description('The resource ID of the Key Vault containing the certificate secret.')
@minLength(1)
param keyVaultId string

@description('The name of the secret in Key Vault containing the certificate.')
@minLength(1)
param keyVaultSecretName string

@description('The resource ID of the App Service Plan (server farm).')
@minLength(1)
param serverFarmId string

@description('The canonical name (CNAME) for domain validation.')
@minLength(1)
param canonicalName string

@description('The domain validation method. Allowed values: email, dns, http. Default is dns.')
@allowed([
  'email'
  'dns'
  'http'
])
param domainValidationMethod string = 'dns'

@description('The list of hostnames for the certificate.')
@minLength(1)
param hostnames array

@description('The PFX certificate blob as a byte array.')
@minLength(1)
param pfxBlob array

var location = resourceGroup().location

resource name_resource 'Microsoft.Web/certificates@2023-12-01' = {
  name: name
  location: location
  tags: empty(tags) ? null : tags
  properties: {
    hostNames: hostnames
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
