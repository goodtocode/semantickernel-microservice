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
	[string] $VersionToReplace='1.0.0',
	[string] $Major='-1',
	[string] $Minor='-1',
	[string] $Revision='-1',
	[string] $Build='-1',
	[string] $Patch='-1',
	[string] $PreRelease='-1',
	[string] $CommitHash='-1'
)

# ***
# *** Initialize
# ***
if ($IsWindows) { Set-ExecutionPolicy Unrestricted -Scope Process -Force }
$VerbosePreference = 'SilentlyContinue' #'Continue'
if ($MyInvocation.MyCommand -and $MyInvocation.MyCommand.Path) {
	[String]$ThisScript = $MyInvocation.MyCommand.Path
	[String]$ThisDir = Split-Path $ThisScript
	[DateTime]$Now = Get-Date
	Write-Debug "*****************************"
	Write-Debug "*** Starting: $ThisScript on $Now"
	Write-Debug "*****************************"
	# Imports
	Import-Module "$ThisDir/../System.psm1"
} else {
	Write-Verbose "No script file context detected. Skipping module import."
}

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


# Calculate versions using Get-Version.ps1 (pass-through all arguments)
$ThisDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$getVersionScript = Join-Path $ThisDir 'Get-Version.ps1'
$getVersionArgs = @{
	Major = $Major
	Minor = $Minor
	Revision = $Revision
	Build = $Build
	Patch = $Patch
	PreRelease = $PreRelease
	CommitHash = $CommitHash
	VersionToReplace = $VersionToReplace
}
$versionJson = & $getVersionScript @getVersionArgs
$versionObj = $versionJson | ConvertFrom-Json
Write-Debug "Get-Version: $($versionJson | ConvertTo-Json -Depth 10)"

$FileVersion = $versionObj.FileVersion
$AssemblyVersion = $versionObj.AssemblyVersion
$InformationalVersion = $versionObj.InformationalVersion
$SemanticVersion = $versionObj.SemanticVersion
Write-Debug "FileVersion: $FileVersion SemanticVersion: $SemanticVersion AssemblyVersion: $AssemblyVersion InformationalVersion: $InformationalVersion"

# *.csproj C# Project files
Update-ContentsByTag -Path $Path -Value $SemanticVersion -Open '<Version>' -Close '</Version>' -Include *.csproj
Update-ContentsByTag -Path $Path -Value $FileVersion -Open '<FileVersion>' -Close '</FileVersion>' -Include *.csproj
Update-ContentsByTag -Path $Path -Value $AssemblyVersion -Open '<AssemblyVersion>' -Close '</AssemblyVersion>' -Include *.csproj
Update-ContentsByTag -Path $Path -Value $InformationalVersion -Open '<InformationalVersion>' -Close '</InformationalVersion>' -Include *.csproj
# *.props/.targets/Directory.Build.props/targets (common for shared versioning)
Update-ContentsByTag -Path $Path -Value $SemanticVersion -Open '<Version>' -Close '</Version>' -Include *.props,*.targets,Directory.Build.props,Directory.Build.targets
Update-ContentsByTag -Path $Path -Value $SemanticVersion -Open '<PackageVersion>' -Close '</PackageVersion>' -Include *.props,*.targets,Directory.Build.props,Directory.Build.targets
# Package.json version
Update-LineByContains -Path $Path -Contains 'version' -Line """version"": ""$FileVersion""," -Include package.json
# OpenApiConfigurationOptions.cs version
Update-LineByContains -Path $Path -Contains 'Version' -Line "Version = ""$AssemblyVersion""," -Include OpenApiConfigurationOptions.cs
# *.nuspec NuGet packages
Update-ContentsByTag -Path $Path -Value $SemanticVersion -Open '<version>' -Close '</version>' -Include *.nuspec
# Assembly.cs C# assembly manifest
Update-LineByContains -Path $Path -Contains "FileVersion(" -Line "[assembly: FileVersion(""$FileVersion"")]" -Include AssemblyInfo.cs
Update-LineByContains -Path $Path -Contains "AssemblyVersion(" -Line "[assembly: AssemblyVersion(""$AssemblyVersion"")]" -Include AssemblyInfo.cs
# *.vsixmanifest VSIX Visual Studio Templates
Update-TextByContains -Path $Path -Contains "<Identity Id" -Old $VersionToReplace -New $FileVersion -Include *.vsixmanifest

Write-Output $FileVersion
