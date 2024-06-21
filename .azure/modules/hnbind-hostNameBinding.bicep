@minLength(1)
@maxLength(128)
param fqdn string

@minLength(1)
@maxLength(128)
param headlessDn string

@minLength(1)
@maxLength(128)
param siteName string

@minLength(1)
@maxLength(256)
param thumbprint string

var deployFqdn = (empty(fqdn) ? bool('false') : bool('true'))
var deployHeadlessDn = (empty(headlessDn) ? bool('false') : bool('true'))

resource siteName_fqdn 'Microsoft.Web/sites/hostNameBindings@2018-11-01' = if (deployFqdn) {
  name: '${siteName}/${fqdn}'
  location: resourceGroup().location
  properties: {
    siteName: siteName
    sslState: 'SniEnabled'
    thumbprint: thumbprint
  }
}

resource siteName_headlessDn 'Microsoft.Web/sites/hostNameBindings@2018-11-01' = if (deployHeadlessDn) {
  name: '${siteName}/${headlessDn}'
  location: resourceGroup().location
  properties: {
    siteName: siteName
    sslState: 'SniEnabled'
    thumbprint: thumbprint
  }
}