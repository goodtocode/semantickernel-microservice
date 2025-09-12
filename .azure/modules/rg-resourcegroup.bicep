targetScope='subscription'

param name string
param location string
param tags object = {}

resource rgResource 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: name
  location: location
  tags: empty(tags) ? null : tags
}
