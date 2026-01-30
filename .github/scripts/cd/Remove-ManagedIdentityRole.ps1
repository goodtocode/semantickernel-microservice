#-----------------------------------------------------------------------
# Remove-ManagedIdentityRole [ObjectId [<String>]] [ApplicationId [<String>]] [Permission [<String>]] 
#
# Example: .\Remove-ManagedIdentityRole -ObjectId 00000000-0000-0000-0000-000000000000 -ApplicationId 00000000-0000-0000-0000-000000000000
#                   -Permission User.Read.All
#
# The app IDs of the Microsoft APIs are the same in all tenants:
#   Microsoft Graph: 00000003-0000-0000-c000-000000000000
#   SharePoint Online: 00000003-0000-0ff1-ce00-000000000000
#-----------------------------------------------------------------------

# ***
# *** Parameters
# ***
[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$ObjectId=$(throw 'ObjectId is a required parameter. (Your Managed Identity Object Id to get new roles)'),
    [Parameter(Mandatory = $true)][string]$ApplicationId=$(throw 'ApplicationId is a required parameter. (The Application Id of the resource to access)'),
    [Parameter(Mandatory = $true)][string]$Permission=$(throw 'Permission is a required parameter. (I.e. "User.Read.All", "User.Invite.All", "GroupMember.ReadWrite.All")')
)

Connect-AzureAD
$app = Get-AzureADServicePrincipal -Filter "AppId eq '$ApplicationId'"
$appRoles = Get-AzureADServiceAppRoleAssignment -ObjectId $app.ObjectId | where PrincipalId -eq $ObjectId
foreach ($appRole in $appRoles) {
    $role = $app.AppRoles | where Id -eq $appRole.Id | Select-Object -First 1
    if ($Permission.Contains($role.Value)) {
        Remove-AzureADServiceAppRoleAssignment -ObjectId $app.ObjectId -AppRoleAssignmentId $appRole.ObjectId
    }    
}
