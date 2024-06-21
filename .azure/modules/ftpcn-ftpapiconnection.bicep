param name string = 'azureblob'
param ftpServerAddress string
param ftpServerPort string
param ftpUsername string
param ftpPassword string

var locationShortName = toLower(replace(resourceGroup().location, ' ', ''))
var nameLower_var = toLower(replace(replace(name, '-', ''), ' ', ''))
var ftpIsSsl = 'true'
var ftpIsBinaryTransport = 'true'
var acceptAnySshHostKey = 'true'

resource nameLower 'Microsoft.Web/connections@2016-06-01' = {
  name: nameLower_var
  location: locationShortName
  kind: 'V1'
  scale: null
  properties: {
    displayName: name
    customParameterValues: {}
    api: {
      name: '${nameLower_var}sftpwithssh'
      displayName: 'SFTP - SSH'
      description: 'SFTP (SSH File Transfer Protocol) is a network protocol that provides file access, file transfer, and file management over any reliable data stream. It was designed by the Internet Engineering Task Force (IETF) as an extension of the Secure Shell protocol (SSH) version 2.0 to provide secure file transfer capabilities.'
      iconUri: 'https://connectoricons-prod.azureedge.net/releases/v1.0.1518/1.0.1518.2564/sftpwithssh/icon.png'
      brandColor: '#e8bb00'
      id: subscriptionResourceId('Microsoft.Web/locations/managedApis', locationShortName, 'ftp')
      type: 'Microsoft.Web/locations/managedApis'
    }
    parameterValues: {
      serverAddress: ftpServerAddress
      userName: ftpUsername
      password: ftpPassword
      serverPort: ftpServerPort
      isssl: false
      disableCertificateValidation: true
    }
  }
  dependsOn: []
}