# Log in to Azure
az login

# Login
az login

# Variables for resource groups, templates, and parameters
$mgmtRg = "can-hubmgmt-plat-wus2-001-rg"
$mgmtTemplate = "bicep/templates/platform-hub-publicroute-mgmt.bicep"
$mgmtParams = "bicep/variables/platform-hub-publicroute-mgmt.bicepparam"
$networkRg = "can-hubnetwork-plat-wus2-001-rg"
$networkTemplate = "bicep/templates/platform-hub-publicroute-network.bicep"
$networkParams = "bicep/variables/platform-hub-publicroute-network.bicepparam"
$hubSubId = "<HubSubID>"
$spokeSubId = "<SpokeSubID>"
$spokeRg = "<spoke-rg>"
$hubVnetName = "<hub-vnet-name>"
$spokeVnetName = "<spoke-vnet-name>"
$hubVnetResourceId = "/subscriptions/$hubSubId/resourceGroups/$networkRg/providers/Microsoft.Network/virtualNetworks/$hubVnetName"
$spokeVnetResourceId = "/subscriptions/$spokeSubId/resourceGroups/$spokeRg/providers/Microsoft.Network/virtualNetworks/$spokeVnetName"
$spokeToHubPeeringName = "spoke-to-hub"
$hubToSpokePeeringName = "hub-to-spoke"
$peeringTemplate = "bicep/modules/vnetpeer-virtualnetworkpeering.bicep"

# Create resource groups if not exist
az group create --name $mgmtRg --location westus2
az group create --name $networkRg --location westus2
az group create --name $spokeRg --location westus2

# Management group deployment: what-if, then deploy if OK
az account set --subscription $hubSubId
az deployment group what-if --resource-group $mgmtRg --template-file $mgmtTemplate --parameters @$mgmtParams
if ($LASTEXITCODE -eq 0) {
   az deployment group create --resource-group $mgmtRg --template-file $mgmtTemplate --parameters @$mgmtParams
}

# Network group deployment: what-if, then deploy if OK
az deployment group what-if --resource-group $networkRg --template-file $networkTemplate --parameters @$networkParams
if ($LASTEXITCODE -eq 0) {
   az deployment group create --resource-group $networkRg --template-file $networkTemplate --parameters @$networkParams
}

# Spoke to Hub Peering
az account set --subscription $spokeSubId
az deployment group create `
   --resource-group $spokeRg `
   --template-file $peeringTemplate `
   --parameters localVnetName=$spokeVnetName peeringName=$spokeToHubPeeringName remoteVnetId=$hubVnetResourceId allowGatewayTransit=false useRemoteGateways=true allowVnetAccess=true allowForwardedTraffic=false

# Hub to Spoke Peering
az account set --subscription $hubSubId
az deployment group create `
   --resource-group $networkRg `
   --template-file $peeringTemplate `
   --parameters localVnetName=$hubVnetName peeringName=$hubToSpokePeeringName remoteVnetId=$spokeVnetResourceId allowGatewayTransit=true useRemoteGateways=false allowVnetAccess=true allowForwardedTraffic=false
