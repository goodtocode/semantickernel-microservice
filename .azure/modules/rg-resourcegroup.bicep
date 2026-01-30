targetScope='subscription'


@description('The name of the Resource Group. Must be 1-90 characters, using only alphanumeric characters, hyphens, underscores, parentheses, and periods.')
@minLength(1)
@maxLength(90)
param name string

@description('The Azure region where the Resource Group will be deployed.')
param location string

@description('Tags to apply to the Resource Group.')
param tags object = {}

resource rgResource 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: name
  location: location
  tags: empty(tags) ? null : tags
}
