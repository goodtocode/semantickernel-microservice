@minLength(1)
param name string

@minLength(1)
param sku string = 'Standard_LRS'

var storageAccountName_var = name

resource storageAccountName 'Microsoft.Storage/storageAccounts@2021-06-01' = {
  name: storageAccountName_var
  location: resourceGroup().location
  tags: {
    displayName: storageAccountName_var
  }
  sku: {
    name: sku
  }
  kind: 'Storage'
}