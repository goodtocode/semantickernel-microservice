using '../templates/platform-spoke-mgmt.bicep'

// =====================
// Common
// =====================
var tenantIac = 'can'
var productIac = 'spokemgmt'
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
// Platform Hub Management RG: ${tenantIac}-${productIac}-${environmentIac}-${regionIac}-${instanceIac}-rg
// i.e. gtc-hubmgmt-plat-wus2-001
// Note: Sentinel is the shared workspace log analytics
// =====================
param hubMgmtSubscriptionId = '00000000-0000-0000-0000-000000000000'
param hubMgmtResourceGroupName = 'hubmgmt-plat-${regionIac}-${instanceIac}-rg'
param workName = 'hubmgmt-plat-${regionIac}-${instanceIac}-sent'

// =====================
// Platform Spoke Management RG: ${tenantIac}-${productIac}-${environmentIac}-${regionIac}-${instanceIac}-rg
// i.e. gtc-spokemgmt-dev-wus2-001
// =====================
param appiName = '${productIac}-${environmentIac}-${regionIac}-${instanceIac}-appi'
param appcsName = '${productIac}-${environmentIac}-${regionIac}-${instanceIac}-appcs'
param appcsSku = 'free'
param planSku = 'F1'
param planName = '${productIac}-${environmentIac}-${regionIac}-${planSku}-${instanceIac}-plan'
