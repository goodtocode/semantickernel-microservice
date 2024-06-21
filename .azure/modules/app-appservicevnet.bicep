@description('The location in which all resources should be deployed.')
param location string = resourceGroup().location

@description('The name of the app to create.')
param appName string = 'appName'

resource existingserverfarm 'Microsoft.Web/serverfarms@2023-01-01' existing={
  name:'existingserverfarm'
}

resource vnetname 'Microsoft.Network/virtualNetworks@2023-06-01' existing ={
  name:'vnetname'
}
resource subnet 'Microsoft.Network/virtualNetworks/subnets@2023-06-01' existing ={
  name:'subnet'
  parent: vnetname

}

resource webApp 'Microsoft.Web/sites@2023-01-01' = {
  name: appName
  location: location
  kind: 'app'
  properties: {
    serverFarmId: existingserverfarm.id
    virtualNetworkSubnetId:subnet.id
    httpsOnly: true
    siteConfig: {
      http20Enabled: true
    }
  }
}
