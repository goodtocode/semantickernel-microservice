
@description('The fully qualified domain name (FQDN) to bind to the App Service. Must be 1-128 characters.')
@minLength(1)
@maxLength(128)
param fqdn string

@description('The headless domain name to bind to the App Service. Must be 1-128 characters.')
@minLength(1)
@maxLength(128)
param headlessDn string

@description('The name of the App Service site. Must be 1-128 characters.')
@minLength(1)
@maxLength(128)
param siteName string

@description('The certificate thumbprint for SSL binding. Must be 1-256 characters.')
@minLength(1)
@maxLength(256)
param thumbprint string

var deployFqdn = (empty(fqdn) ? bool('false') : bool('true'))
var deployHeadlessDn = (empty(headlessDn) ? bool('false') : bool('true'))

resource siteName_fqdn 'Microsoft.Web/sites/hostNameBindings@2023-12-01' = if (deployFqdn) {
  name: '${siteName}/${fqdn}'
  properties: {
    siteName: siteName
    sslState: 'SniEnabled'
    thumbprint: thumbprint
  }
}

resource siteName_headlessDn 'Microsoft.Web/sites/hostNameBindings@2023-12-01' = if (deployHeadlessDn) {
  name: '${siteName}/${headlessDn}'
  properties: {
    siteName: siteName
    sslState: 'SniEnabled'
    thumbprint: thumbprint
  }
}
