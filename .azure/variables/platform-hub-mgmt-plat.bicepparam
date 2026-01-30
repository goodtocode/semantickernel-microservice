using '../templates/platform-hub-mgmt.bicep'

// =====================
// Common
// =====================
var tenantIac = 'can'
var productIac = 'hubmgmt'
var environmentIac = 'plat'
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
// Management RG: ${tenantIac}-${productIac}-${environmentIac}-${regionIac}-${instanceIac}-rg
// rg: gtc-hubmgmt-plat-wus2-001
// =====================
param sentName = '${productIac}-${environmentIac}-${regionIac}-${instanceIac}-sent'
param sentSku = 'PerGB2018'
param appiName = '${productIac}-${environmentIac}-${regionIac}-${instanceIac}-appi'
param kvName = '${productIac}-${environmentIac}-${instanceIac}-kv'
param kvSku = 'standard'
