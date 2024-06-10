#-----------------------------------------------------------------------
# Allow exectution: Set-ExecutionPolicy Unrestricted -Scope Process -Force
# Compress-Vstemplate [-Path [<String>]]
#
# Example: .\Compress-Vstemplate -Path .\vstemplate
#-----------------------------------------------------------------------

# ***
# *** Parameters
# ***
param
(
    [string] $Path=".\vstemplate",
    [string] $Destination="..\vsix\Microservices.Vsix\ProjectTemplates",
	[string] $File="Microservices.zip",
	[boolean] $IsWindows=$true
)

# ***
# *** Initialize
# ***
if ($IsWindows) { Set-ExecutionPolicy Unrestricted -Scope Process -Force }
$VerbosePreference = 'SilentlyContinue' #'Continue'
[String]$ThisScript = $MyInvocation.MyCommand.Path
[String]$ThisDir = Split-Path $ThisScript
[DateTime]$Now = Get-Date
Set-Location $ThisDir # Ensure our location is correct, so we can use relative paths
Write-Host "*****************************"
Write-Host "*** Starting: $ThisScript on $Now"
Write-Host "*****************************"
# Imports
Import-Module ".\Helpers.Code.psm1"
Import-Module ".\Helpers.System.psm1"

# ***
# *** Validate and cleanse
# ***
If($Path.Length -lt 1) { $Path = "$ThisDir\vstemplate"}
If(Compare-IsFirst -String $Path -BeginsWith ".\") {
    $Path = Remove-Prefix -String $Path -Remove ".\"
    $Path = "$ThisDir\$Path"
}
$Path = Remove-Suffix -String $Path -Remove "\"
$Path = Remove-Suffix -String $Path -Remove "/"
If($Destination.Length -lt 1) { $Destination = $ThisDir}
If(Compare-IsFirst -String $Destination -BeginsWith ".\") {
    $Destination = Remove-Prefix -String $Destination -Remove ".\"
    $Destination = "$ThisDir\$Destination"
}
$Destination = Remove-Suffix -String $Destination -Remove "\"
$Destination = Remove-Suffix -String $Destination -Remove "/"
$File = Remove-Prefix -String $File -Remove "\"
$File = Remove-Prefix -String $File -Remove "/"

# ***
# *** Locals
# ***

# ***
# *** Execute
# ***
Remove-Subfolders -Path $Path -Subfolder "net6.0"
Remove-Subfolders -Path $Path -Subfolder "netstandard2.1"
Remove-Subfolders -Path $Path -Subfolder "Debug"
Remove-Subfolders -Path $Path -Subfolder "Release"
Remove-Subfolders -Path $Path -Subfolder "bin"
Remove-Subfolders -Path $Path -Subfolder "obj"

Compress-Path -Path $Path -File "$ThisDir\$File"

Copy-Item -Path "$ThisDir\$File" -Destination $Destination
Remove-Item -Path "$ThisDir\$File"