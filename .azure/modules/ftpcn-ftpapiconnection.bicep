
@description('The name of the FTP API Connection. Must be 1-80 characters, using only alphanumeric characters and hyphens. Default is azureblob.')
@minLength(1)
@maxLength(80)
param name string = 'azureblob'

@description('The address of the FTP server.')
@minLength(1)
@maxLength(255)
param ftpServerAddress string

@description('The port of the FTP server. Default is 21.')
@minLength(1)
@maxLength(5)
param ftpServerPort string = '21'

@description('The username for the FTP server.')
@minLength(1)
@maxLength(128)
param ftpUsername string

@description('The password for the FTP server.')
@minLength(1)
@maxLength(128)
@secure()
param ftpPassword string

@description('Tags to apply to the FTP API Connection resource.')
param tags object = {}

var locationShortName = toLower(replace(resourceGroup().location, ' ', ''))
var nameLower = toLower(replace(replace(name, '-', ''), ' ', ''))

resource connection 'Microsoft.Web/connections@2016-06-01' = {
  name: nameLower
  location: locationShortName
  tags: tags
  properties: {
    displayName: name
    customParameterValues: {}
    api: {
      name: '${nameLower}sftpwithssh'
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
    }
  }
  dependsOn: []
}
