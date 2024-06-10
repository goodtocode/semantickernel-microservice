#-----------------------------------------------------------------------
# Add-CopyrightApache [-Path [<String>]]
#
# Example: .\Add-CopyrightApache -Path \\source\path\MyFramework.sln
#-----------------------------------------------------------------------
function Add-CopyrightApache
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[string[]]$Include = "*.cs",
		[string[]]$Exclude = "",
		[Int32]$First = 100

		)
	$Path = Set-Unc -Path $Path
	# *** Change Destination code Headers
	[String]$OldLicense1 = '//      Copyright (c) MySundial. All rights reserved.'
	[String]$OldLicense2 = '//      All rights are reserved. Reproduction or transmission in whole or in part, in'
	[String]$OldLicense3 = '//      any form or by any means, electronic, mechanical or otherwise, is prohibited'
	[String]$OldLicense4 = '//      without the prior written consent of the copyright owner.'
	[String]$NewLicense1 = '//      '
	[String]$NewLicense2 = '//      Licensed to the Apache Software Foundation (ASF) under one or more ' + [Environment]::NewLine `
		+ '//      contributor license agreements.  See the NOTICE file distributed with ' + [Environment]::NewLine `
		+ '//      this work for additional information regarding copyright ownership.' + [Environment]::NewLine `
		+ '//      The ASF licenses this file to You under the Apache License, Version 2.0 ' + [Environment]::NewLine `
		+ '//      (the ''License''); you may not use this file except in compliance with '
	[String]$NewLicense3 = '//      the License.  You may obtain a copy of the License at ' + [Environment]::NewLine `
		+ '//       ' + [Environment]::NewLine `
		+ '//        http://www.apache.org/licenses/LICENSE-2.0 ' + [Environment]::NewLine `
		+ '//       ' + [Environment]::NewLine `
		+ '//       Unless required by applicable law or agreed to in writing, software  '
	[String]$NewLicense4 = '//       distributed under the License is distributed on an ''AS IS'' BASIS, ' + [Environment]::NewLine `
		+ '//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  ' + [Environment]::NewLine `
		+ '//       See the License for the specific language governing permissions and  ' + [Environment]::NewLine `
		+ '//       limitations under the License. '
	if (test-folder -Path $Path) {
		# Get all children in a folder
		$CodeFiles=get-childitem  -Path $Path -Include $Include  -Recurse -Force | select -First $First
		$Affected = 0
		# Iterate through child files
		foreach ($Item in $CodeFiles)
		{
			# Change file contents
			(Get-Content $Item.PSPath) | 
			Foreach-Object {$_-replace $OldLicense1, $NewLicense1 -replace $OldLicense2, $NewLicense2 -replace $OldLicense3, $NewLicense3 -replace $OldLicense4, $NewLicense4
			} | 
			Set-Content $Item.PSPath
			$Affected = $Affected + 1
		}
		Write-Verbose "[Success] $Affected items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Host "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."
	}
}
export-modulemember -function Add-CopyrightApache

#-----------------------------------------------------------------------
# Clear-Solution [-Path [<String>]]
# [-Include [<String[]>] [-Exclude [<String[]>]]
#
# Example: .\Clear-Solution \\source\path
#-----------------------------------------------------------------------
function Clear-Solution
{
	param (
	 [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
	 [string]$Path = $(throw '-Path is a required parameter.'),
	 [string[]]$Include = ("*.snk", "*.zip", "*.log", "*.bak", "*.tmp,  *.vspscc", "*.vssscc", "*.csproj.vspscc", "*.sqlproj.vspscc", "*.cache"),
	 [string]$Exclude = ""
	)
	Write-Host "Clear-Solution -Path $Path -Include $Include -Exclude $Exclude"
	$Path = Set-Unc -Path $Path
	# Cleanup Files
	Remove-Recurse -Path $Path -Include $Include -Exclude $Exclude
	# Cleanup Folders
	Remove-Subfolders -Path $Path -Subfolder "TestResults"
	Remove-Subfolders -Path $Path -Subfolder ".vs"
	Remove-Subfolders -Path $Path -Subfolder "packages"
	Remove-Subfolders -Path $Path -Subfolder "bin"
	Remove-Subfolders -Path $Path -Subfolder "obj"
}
export-modulemember -function Clear-Solution

#-----------------------------------------------------------------------
# Clear-Lib [-Path [<String>]]
# [-Include [<String[]>] [-Exclude [<String[]>]]
#
# Example: .\Clear-Lib \\source\path
#-----------------------------------------------------------------------
function Clear-Lib
{
	param (
	 [Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
	 [string]$Path = $(throw '-Path is a required parameter.'),
	 [string[]]$Include = ("*.snk", "*.zip", "*.log", "*.bak", "*.tmp,  *.vspscc", "*.vssscc", "*.csproj.vspscc", "*.sqlproj.vspscc", "*.cache"),
	 [string]$Exclude = ""
	)
	Write-Host "Clear-Lib -Path $Path -Include $Include -Exclude $Exclude"
	$Path = Set-Unc -Path $Path
	$Path = Add-Prefix -String $Path -Add "\\"
	# Cleanup Files
	Remove-Recurse -Path $Path -Include $Include -Exclude $Exclude
	# Cleanup Folders
	Remove-Subfolders -Path $Path -Subfolder "TestResults"
	Remove-Subfolders -Path $Path -Subfolder ".vs"
	Remove-Subfolders -Path $Path -Subfolder "packages"
	Remove-Subfolders -Path $Path -Subfolder "bin"
	Remove-Subfolders -Path $Path -Subfolder "obj"
	Remove-Subfolders -Path $Path -Subfolder "app_data"
}
export-modulemember -function Clear-Lib

#-----------------------------------------------------------------------
# Compress-Solution [-Path [<String>]] [-Destination [<String>]] 
#	[-Include [<String[]>] [-Exclude [<String[]>]]
#					[-RepoName [<String>]] [-File [<String>]]
#
# Example: .\Compress-Solution \\source\path \\destination\path
#-----------------------------------------------------------------------
function Compress-Solution
{
	param ( 
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path=$(throw '-Path is a required parameter.'),
		[string]$Destination=$(throw '-Destination is a required parameter.'),
		[String]$RepoName=$(throw '-RepoName is a required parameter.'),		
		[String]$Lib=("\lib"),
		[String]$Build = '',
		[String]$File = ''
	)
	Write-Host "Compress-Solution -Path $Path -Destination $Destination -RepoName $RepoName -Build $Build -File $File"
	# ***
	# *** Validate and cleanse
	# ***
	$Path = Set-Unc -Path $Path
	$Destination = Set-Unc -Path $Destination 
	
	# ***
	# *** Locals
	# ***
	if(-not($File -and $File.Contains(".zip"))) { $File = [String]::Format("{0}.zip", $RepoName) }
	$ZipFile = [string]::Format("{0}\{1}", $Destination, $File)
	Clear-Solution -Path ($Path + '\src\')
	Compress-Path -Path $Path -File $ZipFile
}
export-modulemember -function Compress-Solution

#-----------------------------------------------------------------------
# Get-Version [-Major [<String>]] [-Minor [<String>]]] [-Revision [<String>]]] [-Build [<String>]]]
#  - Convention: Major is any int16. Minor is year based. Revision is month based. Build is hour based.
# Example: .\Get-Version -Major 4
#-----------------------------------------------------------------------
function Get-Version
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$Major = $(throw '-Major is a required parameter.'),
		[String]$Minor = '',
		[String]$Revision = '',
		[String]$Build = '',
		[String]$Format = 'M.YY.MM.HHH'
	)	
	Write-Host "Get-Version -Major $Major -Minor $Minor -Revision $Revision -Build $Build"
	[DateTime]$Now = Get-Date
	[DateTime]$BoM = Get-Date -Year $Now.Year -Month $Now.Month -Day 1 -Hour 0 -Minute 0 -Second 0
	[String]$returnValue = ''

	$Minor = $Minor.Replace('-1', '')
	$Revision = $Revision.Replace('-1', '')
	$Build = $Build.Replace('-1', '')
	$TimeSpan = $Now - $Bom
	$HoursSoFar = ($TimeSpan.Days * 24) + $TimeSpan.Hours;
	[String] $YY = $Now.Year.ToString().Substring(2, 2).PadLeft(2, "0")
	[String] $MM = $Now.Month.ToString().PadLeft(2, "0")
	# Default: M.YYYY.MM.HHH
	if($Minor -eq '') { $Minor = $YY}
	if($Revision -eq '') { $Revision = "$YY$MM" }
	if($Build -eq '') { 
		$Build = [String]::Format("{0}{1}1", $HoursSoFar.ToString().PadLeft(4, "0"), $Now.Minute.ToString().PadLeft(2, "0")).ToString().PadLeft(6, "0") 
		if (Compare-IsLast -String $Build -EndsWith '0'){
			Remove-Suffix -String $Build -Remove '0'
			Add-Suffix -String $Build -Add '1'
		}
	}
	$returnValue = [String]::Format("{0}.{1}.{2}.{3}", $Major, $Minor, $Revision, $Build)
	# Override default if need
	if($Format -eq 'M.YY.MM')
	{
		$returnValue = [String]::Format("{0}.{1}.{2}", $Major, $Minor, $Revision)
	}
	elseif($Format -eq 'YYYY.MM')
	{
		$returnValue = [String]::Format("{0}.{1}", $Now.Year, $Revision)
	}
	
	return $returnValue
}
export-modulemember -function Get-Version

#-----------------------------------------------------------------------
# Set-Version [-Path [<String>]]a [-Contains [<String[]>] [-Close [<String[]>]]
#
# Example: .\Set-Version -Path \\source\path
#-----------------------------------------------------------------------
function Set-Version
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[Version] $VersionToReplace = $(throw '-VersionToReplace is a required parameter. i.e. 1.20.1'),
		[String]$Major = '1',
		[String]$Minor = '',
		[String]$Revision = '',
		[String]$Build = ''
	)
	Write-Host "Set-Version -Path $Path"
	# .Net Projects
	$Version = Get-Version -Major $Major -Minor $Minor -Revision $Revision -Build $Build
	Update-ContentsByTag -Path $Path -Value $Version -Open '<version>' -Close '</version>' -Include *.nuspec
	Update-LineByContains -Path $Path -Contains "AssemblyVersion(" -Line "[assembly: AssemblyVersion(""$Version"")]" -Include AssemblyInfo.cs
	# Vsix Templates
	$OldVersion = Get-Version -Major $Major -Minor ($Now.Month - 1).ToString("00") -Format 'M.YY.MM'
	$Version = Get-Version -Major $Major -Format 'M.YY.MM.HHH'
	Update-Text -Path $Path -Old $VersionToReplace -New $Version -Include *.vsixmanifest
	# No NuGet Version Needed - handled by that individual process
}
export-modulemember -function Set-Version

#-----------------------------------------------------------------------
# Find-MsBuild [-Path [<String>]]
#
# Example: .\Find-MsBuild
#	Result: 
#-----------------------------------------------------------------------
function Find-DevEnv
{
	param (
		[int] $Year = 2017
	)
	Write-Host "Find-DevEnv -Year $Year"
	$ExeName = 'DevEnv.exe'
	[int] $FolderYear = 2017;
	if($Year -gt 2016) {$FolderYear = $Year}
    $devPath = "$Env:programfiles (x86)\Microsoft Visual Studio\$FolderYear\Enterprise\Common7\IDE\$ExeName"
    $proPath = "$Env:programfiles (x86)\Microsoft Visual Studio\$FolderYear\Professional\Common7\IDE\$ExeName"
    $communityPath = "$Env:programfiles (x86)\Microsoft Visual Studio\$FolderYear\Community\Common7\IDE\$ExeName"
    $fallback2015Path = "${Env:ProgramFiles(x86)}\Microsoft Visual Studio 14.0\Common7\IDE\$ExeName"
    $fallback2013Path = "${Env:ProgramFiles(x86)}\Microsoft Visual Studio 12.0\Common7\IDE\$ExeName"
	$fallback2010Path = "${Env:ProgramFiles(x86)}\Microsoft Visual Studio 10.0\Common7\IDE\$ExeName"
		
    If ((2017 -le $Year) -And (Test-Path $devPath)) { return $devPath } 
    If ((2017 -le $Year) -And (Test-Path $proPath)) { return $proPath } 
    If ((2017 -le $Year) -And (Test-Path $communityPath)) { return $communityPath } 
    If ((2015 -le $Year) -And (Test-Path $fallback2015Path)) { return $fallback2015Path } 
    If ((2013 -le $Year) -And (Test-Path $fallback2013Path)) { return $fallback2013Path } 
	If ((2010 -le $Year) -And (Test-Path $fallback2010Path)) { return $fallback2010Path } 
}
export-modulemember -function  Find-DevEnv

#-----------------------------------------------------------------------
# Find-MsBuild [-Path [<String>]]
#
# Example: .\Find-MsBuild
#	Result: 
#-----------------------------------------------------------------------
function Find-MsBuild
{
	param (
		[int]$Year = 2017,
		[string]$Version = '15.0'
	)
	Write-Host "Find-MsBuild -Year $Year - Version $Version"
	$ExeName = 'MsBuild.exe'
	[int] $FolderYear = 2017;
	if($Year -gt 2016) {$FolderYear = $Year}
    $agentPath = "$Env:programfiles (x86)\Microsoft Visual Studio\$FolderYear\BuildTools\MSBuild\$Version\Bin\$ExeName"
    $devPath = "$Env:programfiles (x86)\Microsoft Visual Studio\$FolderYear\Enterprise\MSBuild\$Version\Bin\$ExeName"
    $proPath = "$Env:programfiles (x86)\Microsoft Visual Studio\$FolderYear\Professional\MSBuild\$Version\Bin\$ExeName"
    $communityPath = "$Env:programfiles (x86)\Microsoft Visual Studio\$FolderYear\Community\MSBuild\$Version\Bin\$ExeName"
    $fallback2015Path = "${Env:ProgramFiles(x86)}\MSBuild\14.0\Bin\$ExeName"
    $fallback2013Path = "${Env:ProgramFiles(x86)}\MSBuild\12.0\Bin\$ExeName"
    $fallbackPath = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\$ExeName"
		
    If ((2017 -le $Year) -And (Test-Path $agentPath)) { return $agentPath } 
    If ((2017 -le $Year) -And (Test-Path $devPath)) { return $devPath } 
    If ((2017 -le $Year) -And (Test-Path $proPath)) { return $proPath } 
    If ((2017 -le $Year) -And (Test-Path $communityPath)) { return $communityPath } 
    If ((2015 -le $Year) -And (Test-Path $fallback2015Path)) { return $fallback2015Path } 
    If ((2013 -le $Year) -And (Test-Path $fallback2013Path)) { return $fallback2013Path } 
    If (Test-Path $fallbackPath) { return $fallbackPath } 
}
export-modulemember -function  Find-MsBuild

#-----------------------------------------------------------------------
# Remove-Subdomain [-Domain [<String>]]
#
# Example: .\Remove-Subdomain -Domain www.MySundial.com
#	Result: MySundial.com
#-----------------------------------------------------------------------
function Remove-Subdomain
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Domain = $(throw '-Domain is a required parameter.')
	)
	Write-Host "Remove-Subdomain -Domain $Domain"
	[string]$ReturnValue = $Domain
	[Int]$Period1 = -1
	[Int]$Period2 = -1
	
	$Period1 = $Domain.IndexOf('.')	
	if($Period1 -gt 0)
	{
		$Period2 = $Domain.IndexOf('.', $Period1)
		if(($Period1 -le $Period2) -and ($Period1 -le $Domain.Length))
		{
			$ReturnValue = $Domain.Substring($Period1 + 1)
			Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
		}
	}

	if($Period2 -le $Period1)
	{
		Write-Host "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). Not a Uri domain part format"
	}

	return $ReturnValue
}
export-modulemember -function Remove-Subdomain