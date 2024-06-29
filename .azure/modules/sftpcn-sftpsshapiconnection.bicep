param name string = 'azureblob'
param ftpServerAddress string
param ftpServerPort string
param ftpUsername string
param ftpPassword string
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
var nameLower_var = toLower(replace(replace(name, '-', ''), ' ', ''))

resource nameLower 'MICROSOFT.WEB/CONNECTIONS@2018-07-01-preview' = {
  name: nameLower_var
  location: locationShortName
  properties: {
    api: {
      id: '${subscription().id}/providers/Microsoft.Web/locations/${locationShortName}/managedApis/sftpwithssh'
      type: 'Microsoft.Web/locations/managedApis'
      name: '${nameLower_var}sftpwithssh'
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
      acceptAnySshHostKey: ftpAcceptAnySshKey
      sshHostKeyFingerprint: ftpHostKeyFingerprint
      rootFolder: ftpRootFolder
    }
  }
}