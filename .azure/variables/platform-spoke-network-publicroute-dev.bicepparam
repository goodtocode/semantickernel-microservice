using '../templates/platform-spoke-network-publicroute.bicep'

// =====================
// Common
// =====================
var tenantIac = 'can'
var productIac = 'spokenetwork'
var environmentIac = 'dev'
var regionIac = 'wus2'
var instanceIac = '001'
param location = 'westus2'
param tags = {
  Environment: environmentIac
  CostCenter: '0000'
  project: productIac
  owner: tenantIac
}

// =====================
// Networking RG
// =====================
param vnetName = '${productIac}-${environmentIac}-${regionIac}-${instanceIac}-vnet'
param vnetCidr = '10.10.0.0/16'
param snetNameManagement = '${productIac}-hub-management-${instanceIac}-snet'
param snetCidrManagement = '10.10.1.0/24'
