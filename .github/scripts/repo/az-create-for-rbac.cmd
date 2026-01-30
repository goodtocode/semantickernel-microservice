REM
ECHO *** az-create-for-rbac.cmd ***
REM *** Usage: az-create-for-rbac.cmd

REM *** Locals
SET FullPath=%1
SET FullPath=%FullPath:"=%



exit 0



az login

az ad sp create-for-rbac --name "myApp" --role contributor \ 
                        --scopes /subscriptions/{subscription-id}/resourceGroups/{resource-group} \
                        --sdk-auth