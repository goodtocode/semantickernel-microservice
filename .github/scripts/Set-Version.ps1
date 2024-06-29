#-----------------------------------------------------------------------
# Set-Version [-Path [<String>]] [-VersionToReplace [<String>]]  [-Type [<String>]] 
#
# Example: .\Set-Version -Path \\source\path -Major 1
#-----------------------------------------------------------------------

# ***
# *** Parameters
# ***
param
(
	[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
    [string] $Path=$(throw '-Path is a required parameter. i.e. $(Build.SourcesDirectory)'),
	[Version] $VersionToReplace='1.0.0',
	[String] $Major='-1',
	[String] $Minor='-1',
	[String] $Revision='-1',
	[String] $Build='-1',
	[String] $Patch='-1',
	[String] $PreRelease='-1',
	[String] $CommitHash='-1'
)

# ***
# *** Initialize
# ***
if ($IsWindows) { Set-ExecutionPolicy Unrestricted -Scope Process -Force }
$VerbosePreference = 'SilentlyContinue' #'Continue'
[String]$ThisScript = $MyInvocation.MyCommand.Path
[String]$ThisDir = Split-Path $ThisScript
[DateTime]$Now = Get-Date
Write-Debug "*****************************"
Write-Debug "*** Starting: $ThisScript on $Now"
Write-Debug "*****************************"
# Imports
Import-Module "$ThisDir/System.psm1"

# ***
# *** Validate and cleanse
# ***
If($IsWindows){
	$Path = Set-Unc -Path $Path
}

# ***
# *** Locals
# ***

# ***
# *** Execute
# ***
$Major = $Major.Replace('-1', $VersionToReplace.ToString().Substring(0,1)) # Static 1, 2, 3
$Minor = $Minor.Replace('-1', (Get-Date -UFormat '%Y').ToString().Substring(2,2)) # Year YYYY 2023
$Revision = $Revision.Replace('-1', (Get-Date -UFormat '%j').ToString()) # DayOfYear D[DD]1-365
$Build = $Build.Replace('-1', (Get-Date -UFormat '%H%M').ToString()) # HrMin 1937
$Patch = $Patch.Replace('-1', (Get-Date -UFormat '%m').ToString()) # Month mm
$PreRelease = $PreRelease.Replace('-1', '') # -alpha
$CommitHash = $CommitHash.Replace('-1', '') # +204ff0a


# Version Formats
$FileVersion = "$Major.$Minor.$Revision.$Build" # Ref: https://learn.microsoft.com/en-us/dotnet/standard/library-guidance/versioning
$AssemblyVersion = "$Major.$Minor.0.0"
$InformationalVersion = "$Major.$Minor.$Revision$PreRelease$CommitHash"
$SemanticVersion = "$Major.$Minor.$Patch$PreRelease"
Write-Debug "FileVersion: $FileVersion SemanticVersion: $SemanticVersion AssemblyVersion: $AssemblyVersion InformationalVersion: $InformationalVersion"

# Package.json version
Update-LineByContains -Path $Path -Contains 'version' -Line """version"": ""$FileVersion""," -Include package.json
# OpenApiConfigurationOptions.cs version
Update-LineByContains -Path $Path -Contains 'Version' -Line "Version = ""$AssemblyVersion""," -Include OpenApiConfigurationOptions.cs
# *.csproj C# Project files
Update-ContentsByTag -Path $Path -Value $FileVersion -Open '<Version>' -Close '</Version>' -Include *.csproj
# *.nuspec NuGet packages
Update-ContentsByTag -Path $Path -Value $SemanticVersion -Open '<version>' -Close '</version>' -Include *.nuspec
# Assembly.cs C# assembly manifest
Update-LineByContains -Path $Path -Contains "FileVersion(" -Line "[assembly: FileVersion(""$FileVersion"")]" -Include AssemblyInfo.cs
Update-LineByContains -Path $Path -Contains "AssemblyVersion(" -Line "[assembly: AssemblyVersion(""$AssemblyVersion"")]" -Include AssemblyInfo.cs
# *.vsixmanifest VSIX Visual Studio Templates
Update-TextByContains -Path $Path -Contains "<Identity Id" -Old $VersionToReplace -New $FileVersion -Include *.vsixmanifest

Write-Output $FileVersion
