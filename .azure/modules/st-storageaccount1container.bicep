@description('Name of the Storage Account. (st)')
@minLength(3)
@maxLength(24)
param name string

@description('Sku of the Storage Account.')
@allowed([
  'Premium_LRS'
  'Premium_ZRS'
  'Standard_GRS'
  'Standard_GZRS'
  'Standard_LRS'
  'Standard_RAGRS'
  'Standard_RAGZRS'
  'Standard_ZRS'
])
param sku string = 'Standard_LRS'

@description('Kind of the Storage Account.')
@allowed([
  'BlobStorage'
  'BlockBlobStorage'
  'FileStorage'
  'Storage'
  'StorageV2'
])
param kind string = 'StorageV2'

@description('Allow public access')
param allowBlobPublicAccess bool = false

@description('Name of the Storage Account container 1')
@minLength(3)
@maxLength(24)
param container1Name string

@description('Name of the Storage Account container 1')
@minLength(3)
@maxLength(24)
@allowed([
  'Container'
  'None'
])
param container1Access string = 'Container'

resource name_resource 'Microsoft.Storage/storageAccounts@2021-06-01' = {
  name: name
  location: resourceGroup().location
  tags: {
    displayName: name
  }
  sku: {
    name: sku
  }
  kind: kind
  properties: {
    supportsHttpsTrafficOnly: true
    allowBlobPublicAccess: allowBlobPublicAccess
  }
}

resource name_default_container1Name 'Microsoft.Storage/storageAccounts/blobServices/containers@2018-03-01-preview' = {
  parent: name_default
  name: container1Name
  properties: {
    publicAccess: container1Access
  }
  dependsOn: [
    name_resource
  ]
}

resource name_default 'Microsoft.Storage/storageAccounts/blobServices@2021-06-01' = {
  parent: name_resource
  name: 'default'
  sku: {
    name: sku
  }
  properties: {
    cors: {
      corsRules: []
    }
  }
}

resource Microsoft_Storage_storageAccounts_fileServices_name_default 'Microsoft.Storage/storageAccounts/fileServices@2021-06-01' = {
  parent: name_resource
  name: 'default'
  sku: {
    name: sku
  }
  properties: {
    cors: {
      corsRules: []
    }
  }
}

resource Microsoft_Storage_storageAccounts_queueServices_name_default 'Microsoft.Storage/storageAccounts/queueServices@2021-06-01' = {
  parent: name_resource
  name: 'default'
  properties: {
    cors: {
      corsRules: []
    }
  }
}

resource Microsoft_Storage_storageAccounts_tableServices_name_default 'Microsoft.Storage/storageAccounts/tableServices@2021-06-01' = {
  parent: name_resource
  name: 'default'
  properties: {
    cors: {
      corsRules: []
    }
  }
}