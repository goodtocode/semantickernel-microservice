$rgName='rg-PRODUCT-ENVIRONMENT-001'
$rgLocation='westus2'
# rg-
New-AzResourceGroup -Name $rgName -Location $rgLocation

# apim-
New-AzResourceGroupDeployment -ResourceGroupName $rgName -TemplateFile ./bicep/apim-apimanagement.bicep -publisherEmail "<publisher-email>" -publisherName "<publisher-name>"