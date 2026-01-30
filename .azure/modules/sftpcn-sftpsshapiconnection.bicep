
@description('The name of the SFTP SSH API Connection. Must be 1-80 characters, using only alphanumeric characters and hyphens. Default is azureblob.')
@minLength(1)
@maxLength(80)
param name string = 'azureblob'

@description('The address of the SFTP server.')
@minLength(1)
@maxLength(255)
param ftpServerAddress string

@description('The port of the SFTP server. Default is 22.')
@minLength(1)
@maxLength(5)
param ftpServerPort string = '22'

@description('The username for the SFTP server.')
@minLength(1)
@maxLength(128)
param ftpUsername string

@description('The password for the SFTP server.')
@minLength(1)
@maxLength(128)
@secure()
param ftpPassword string

@description('The root folder for the SFTP connection. Default is /.')
@minLength(1)
@maxLength(255)
param ftpRootFolder string = '/'

@description('SSH private key (the content of the file should be provided entirely as is, in the multiline format)')
@secure()
param ftpPrivateKey string = ''

@description('SSH private key passphrase (if the private key is protected by a passphrase)')
@secure()
param ftpPassphrase string = ''

@description('Disable SSH host key validation? (True/False)')
param ftpAcceptAnySshKey bool = true

@description('SSH host key finger-print')
param ftpHostKeyFingerprint string = ''

var locationShortName = toLower(replace(resourceGroup().location, ' ', ''))
var nameLower = toLower(replace(replace(name, '-', ''), ' ', ''))

resource connection 'Microsoft.Web/connections@2016-06-01' = {
  name: nameLower
  location: locationShortName
  properties: {
    api: {
      id: '${subscription().id}/providers/Microsoft.Web/locations/${locationShortName}/managedApis/sftpwithssh'
      type: 'Microsoft.Web/locations/managedApis'
      name: '${nameLower}sftpwithssh'
      displayName: 'SFTP - SSH'
      description: 'SFTP (SSH File Transfer Protocol) is a network protocol that provides file access, file transfer, and file management over any reliable data stream. It was designed by the Internet Engineering Task Force (IETF) as an extension of the Secure Shell protocol (SSH) version 2.0 to provide secure file transfer capabilities.'
      iconUri: 'https://connectoricons-prod.azureedge.net/releases/v1.0.1518/1.0.1518.2564/sftpwithssh/icon.png'
      brandColor: '#e8bb00'
    }
    displayName: name
    parameterValues: {
      hostName: ftpServerAddress
      userName: ftpUsername
      password: ftpPassword
      sshPrivateKey: ftpPrivateKey
      sshPrivateKeyPassphrase: ftpPassphrase
      portNumber: ftpServerPort
      acceptAnySshHostKey: string(ftpAcceptAnySshKey)
      sshHostKeyFingerprint: ftpHostKeyFingerprint
      rootFolder: ftpRootFolder
    }
  }
}
