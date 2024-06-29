#-----------------------------------------------------------------------
# Add-Prefix [-String [<String>]]
#
# Example: .\Add-Prefix -String ello -Add H
#	Result: Hello
#-----------------------------------------------------------------------
function Add-Prefix
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$String = $(throw '-String is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Add = $(throw '-Add is a required parameter.')
	)
	Write-Verbose "Add-Prefix -String $String -Add $Add"
	[string]$ReturnValue = $String
	if (-not (Compare-IsFirst -String $String -BeginsWith $Add))
	{ 		
		$ReturnValue = ($Add + $ReturnValue)
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -String $String already has prefix of -Add $Add"
	}
	return $ReturnValue
}
export-modulemember -function Add-Prefix

#-----------------------------------------------------------------------
# Add-Suffix [-String [<String>]]
#
# Example: .\Add-Suffix -String Hell -Add o
#	Result: Hello
#-----------------------------------------------------------------------
function Add-Suffix
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$String = $(throw '-String is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Add = $(throw '-Add is a required parameter.')
	)
	Write-Verbose "Add-Suffix -String $String -Add $Add"
	[string]$ReturnValue = $String
	if (-not (Compare-IsLast -String $String -EndsWith $Add))
	{ 		
		$ReturnValue = ($String + $Add)
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -String $String already has suffix of -Add $Add"
	}

	return $ReturnValue
}
export-modulemember -function Add-Suffix

#-----------------------------------------------------------------------
# Compare-IsFirst [-String [<String>]]
#
# Example: .\Compare-IsFirst -String Hell -EndsWith H
#	Result: false
# Example: .\Compare-IsFirst -String Hello -Add H
#	Result: Hello
#-----------------------------------------------------------------------
function Compare-IsFirst
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$String = $(throw '-String is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$BeginsWith = $(throw '-BeginsWith is a required parameter.')		
	)

	Write-Verbose "Compare-IsFirst -String $String -EndsWith $EndsWith"
	[Boolean]$ReturnValue = $false	
	if($BeginsWith.Length -lt $String.Length)
	{
		$StringBeginning = $String.SubString(0, $BeginsWith.Length).ToLower()
		if ($StringBeginning.ToLower().Equals($BeginsWith.ToLower()))
		{ 		
			$ReturnValue = $true
		}
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	return $ReturnValue
}
export-modulemember -function Compare-IsFirst

#-----------------------------------------------------------------------
# Compare-IsLast [-String [<String>]]
#
# Example: .\Compare-IsLast -String Hell -EndsWith H
#	Result: false
# Example: .\Compare-IsLast -String Hello -Add H
#	Result: Hello
#-----------------------------------------------------------------------
function Compare-IsLast
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$String = $(throw '-String is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$EndsWith = $(throw '-EndsWith is a required parameter.')
	)
	Write-Verbose "Compare-IsLast -String $String -EndsWith $EndsWith"
	[Boolean]$ReturnValue = $false
	if($EndsWith.Length -lt $String.Length)
	{
		$StringEnding = $String.SubString(($String.Length - $EndsWith.Length), $EndsWith.Length)
		if ($StringEnding.ToLower().Equals($EndsWith.ToLower()))
		{ 		
			$ReturnValue = $true
		}
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}

	return $ReturnValue
}
export-modulemember -function Compare-IsLast

#-----------------------------------------------------------------------
# Compress-Path [-Path [<String>]] [-File [<String>]] 
#
# Example: .\Compress-Path \\source\path \\destination\path\file.zip
#-----------------------------------------------------------------------
function Compress-Path
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path=$(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$File=$(throw '-File is a required parameter.')
	)
	Write-Verbose "Compress-Path -Path $Path -File $File"
	New-Path -Path $Path
	Remove-File $File
	[Reflection.Assembly]::LoadWithPartialName("System.IO.Compression.FileSystem")
	[System.IO.Compression.ZipFile]::CreateFromDirectory($Path, $File)
}
export-modulemember -function Compress-Path

#-----------------------------------------------------------------------
# Convert-PathSafe [-Path [<String>]]
#
# Example: .\Convert-PathSafe -Path \\source\path
#-----------------------------------------------------------------------
function Convert-PathSafe
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.')
	)
	Write-Verbose "Convert-PathSafe -Path $Path"
	$Path = $Path.Trim()
	$ReturnValue = $Path
	$Path = Set-Unc -Path $Path
	if(Test-Path -Path $Path)
	{
		$ReturnValue = Convert-Path -Path $Path
		if (-not ($ReturnValue))
		{
			$ReturnValue = $Path
			Write-Verbose "[Warning] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path didnt convert."
		}
	}
	else
	{
		Write-Verbose "[Error] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."		
	}
	return $ReturnValue
}
export-modulemember -function Convert-PathSafe

#-----------------------------------------------------------------------
# Copy-Backup [-Path [<String>]] [-Destination [<String>]]
#
# Example: .\Copy-Backup -Path \\source\path -Destination \\destination\path
#-----------------------------------------------------------------------
function Copy-Backup
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Destination = $(throw '-Destination is a required parameter.')
	)
	Write-Verbose "Copy-Backup -Path $Path -Destination $Destination"	
	$Path = Remove-Suffix -String $Path -Remove "\"
	New-Path -Path $Destination
	[String]$BackupPath=[string]::Format("{0}\{1}", $Destination, (Get-Date).ToString("yyyy-MM-dd"))
	if($Path)
	{
		if(-not (Test-Path -Path $BackupPath -PathType Container)){
			New-Path -Path $BackupPath
		}
		Copy-Recurse -Path $Path -Destination $BackupPath
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path to -Destination $BackupPath"
	}
	else
	{
		Write-Verbose "[Error] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."		
	}
}
export-modulemember -function Copy-Backup

#-----------------------------------------------------------------------
# Copy-File [-Path [<String>]] [-Destination [<String>]]
#
# Example: .\Copy-File -Path \\source\path\File.name -Destination \\destination\path
#-----------------------------------------------------------------------
function Copy-File
{
	param (
		[Parameter(Mandatory = $True)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory = $True)]
		[string]$Destination = $(throw '-Destination is a required parameter.'),
		[string[]]$Include = "*.*",
		[string[]]$Exclude = "",
		[bool]$Overwrite = $true
	)
	Write-Verbose "Copy-File -Path $Path -Destination $Destination -Overwrite $Overwrite"
	$Destination = Set-Unc -Path $Destination
	if(Test-File -Path $Path)
	{
		New-Path -Path $Destination
		$DestinationAbsolute = $Destination		
		if(Test-Folder -Path $Destination)
		{
			$DestinationAbsolute = Convert-PathSafe -Path $Destination	
		}
		$DestinationPathFile = $DestinationAbsolute
		$FolderArray = $Path.Split('\')
		if($FolderArray.Count -gt 0)
		{
			$DestinationPathFile = Join-Path $DestinationAbsolute $FolderArray[$FolderArray.Count-1]
		}
		if((-not (Test-Path $DestinationPathFile -PathType Leaf)) -or ($Overwrite -eq $true))
		{
			try{
				Copy-Item -Path $Path -Destination $DestinationAbsolute -Include $Include -Exclude $Exclude -Force
			}
			catch{
				Write-Verbose "[Error] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."
			}			
		}
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path to -Destination $DestinationAbsolute"
	}
	else
	{
		Write-Verbose "[Error] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."
	}
}
export-modulemember -function Copy-File

#-----------------------------------------------------------------------
# Copy-Recurse [-Source [<String>]] [-Destination [<String>]]
# [-Include [<String[]>] [-Exclude [<String[]>]]
#
# Example: .\Copy-Recurse \\source\path \\destination\path
#-----------------------------------------------------------------------
function Copy-Recurse
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Source is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Destination = $(throw '-Destination is a required parameter.'),
		[string[]]$Include = "*.*",
		[string[]]$Exclude = "",
		[Int32]$First = 1000,
		[bool]$Overwrite=$True,
		[bool]$Clean = $False
	)
	Write-Verbose "Copy-Recurse -Path $Path -Destination $Destination -Include $Include -Exclude $Exclude -First $ -Overwrite $Overwrite -Clean $Clean"
	$Affected = 0
	$Path = Set-Unc -Path $Path
	if (Test-Path $Path)
	{
		$PathAbsolute = Convert-PathSafe -Path $Path
		# Optionally Clean
		if($Clean -eq $True) { Remove-Path -Path $Destination }
		New-Path -Path $Destination
		$DestinationAbsolute =  $Destination
		if(Test-Path $Destination) { $DestinationAbsolute=Convert-PathSafe -Path $Destination }		
		$Items = Get-ChildItem -Path $PathAbsolute -Recurse -Include $Include -Exclude $Exclude | where { ! $_.PSIsContainer }		
		ForEach ($Item in $Items) {
			$PathArray = $PathAbsolute.Split('\') 
			$Folder = $DestinationAbsolute
			for ($count=1; $count -lt $PathArray.length-1; $count++) {
				$Subfolder = $PathArray[$count]
				$Folder = Join-Path $Folder $Subfolder
				if (($Folder.Length > 0) -and (-not (Test-Path $Folder))) {
					Write-Verbose "New-Item -ItemType directory -Force -Path $Folder"
					New-Item -ItemType directory -Force -Path $Folder 
				}
			}
			$DirName = $Item.DirectoryName
			$Position = $DirName.IndexOf($PathAbsolute)
			$PathSegment = $DirName.SubString($Position + $PathAbsolute.Length)
			$NewPath = Join-Path $DestinationAbsolute $PathSegment
			Copy-File -Path $Item.FullName -Destination $NewPath -Overwrite $Overwrite
			$Affected = $Affected + 1
		}
		Write-Verbose "[Success] $Affected items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[Error] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."
	}
}
export-modulemember -function Copy-Recurse

#-----------------------------------------------------------------------
# Expand-File [-Path [<String>]] [-File [<String>]] 
#
# Example: .\Expand-File \\source\path\file.zip \\destination\path
#-----------------------------------------------------------------------
function Expand-File
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path=$(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$File=$(throw '-File is a required parameter.')
	)
	Write-Verbose "Expand-Zip -Path $Path -File $File"
	New-Path -Path $Path
	[Reflection.Assembly]::LoadWithPartialName("System.IO.Compression.FileSystem")
	[System.IO.Compression.ZipFile]::ExtractToDirectory($File, $Path)
}
export-modulemember -function Expand-File

#-----------------------------------------------------------------------
# Find-File [-Path [<String>]] [-File [<String>]] 
#
# Example: .\Find-File \\source\path\file.zip \\destination\path
#-----------------------------------------------------------------------
function Find-File
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path=$(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$File=$(throw '-File is a required parameter.'),
		[Int32]$First=1
	)
	Write-Verbose "Find-Zip -Path $Path -File $File"
	Get-Childitem -Path $Path -Include $File -Recurse| select -First $First
}
export-modulemember -function Find-File

#-----------------------------------------------------------------------
# Get-AssemblyStrongName
#
# Example: Get-AssemblyStrongName
#-----------------------------------------------------------------------
function Get-AssemblyStrongName($assemblyPath)
{
    [System.Reflection.AssemblyName]::GetAssemblyName($assemblyPath).FullName 
}
export-modulemember -function Get-AssemblyStrongName

#-----------------------------------------------------------------------
# Get-CurrentLine
#
# Example: Get-CurrentLine
#-----------------------------------------------------------------------
function Get-CurrentLine { 
 $MyInvocation.ScriptLineNumber 
} 
export-modulemember -function Get-CurrentLine

#-----------------------------------------------------------------------
# Get-CurrentFile
#
# Example: Get-CurrentFile
#-----------------------------------------------------------------------
function Get-CurrentFile { 
 $MyInvocation.ScriptName 
} 
export-modulemember -function Get-CurrentFile

#-----------------------------------------------------------------------
# Get-FilesByString
#
# Example: Get-FilesByString
#-----------------------------------------------------------------------
function Get-FilesByString {  
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[string]$String = $(throw '-String is a required parameter.'),
		[string[]]$Include = ("*.*"),
		[string[]]$Exclude = ""
	)
	Write-Verbose "Get-FilesByString -Path $Path -String $String -Include $Include -Exclude $Exclude"
	$Path = Set-Unc -Path $Path

	$ReturnData = Get-Childitem -Path $Path -Include $Include -Exclude $Exclude -Recurse | Select-String -pattern $String | group path | select name

	return $ReturnData
} 
export-modulemember -function Get-FilesByString

#-----------------------------------------------------------------------
# Get-SystemFolders
#
# Example: Get-SystemFolder
#-----------------------------------------------------------------------
function Get-SystemFolders
{
	param (
	)
	Write-Verbose "Get-SystemFolders"
	$SpecialFolders = @{}
	$names = [Environment+SpecialFolder]::GetNames([Environment+SpecialFolder])
	foreach($name in $names)
	{
	  if($path = [Environment]::GetFolderPath($name)){
 	$SpecialFolders[$name] = $path
	  }
	else
	{
		Write-Verbose "[Warning] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	}
	return $SpecialFolders
}
export-modulemember -function Get-SystemFolders

#-----------------------------------------------------------------------
# Get-SystemFolder [-Name [<String>]]
# Keys: Desktop,Programs,MyDocuments,Personal,Favorites,Startup,Recent,SendTo,StartMenu,MyMusic,MyVideos,DesktopDirectory,NetworkShortcuts,Fonts
#	Templates,CommonStartMenu,CommonPrograms,CommonStartup,CommonDesktopDirectory,ApplicationData,PrinterShortcuts,LocalApplicationData,InternetCache
#	Cookies,History,CommonApplicationData,Windows,System,ProgramFiles,MyPictures,UserProfile,SystemX86,ProgramFilesX86
#	CommonProgramFiles,CommonProgramFilesX86,CommonTemplates,CommonDocuments,CommonAdminTools,AdminTools,CommonMusic,CommonPictures,CommonVideos,ResourcesCDBurning
# Example: Get-SystemFolder -Name 'UserProfile'
#-----------------------------------------------------------------------
function Get-SystemFolder
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$Name=$(throw '-Folder is a required parameter.')
	)
	Write-Verbose "Get-SystemFolder -Folder $Folder"
	if($path = [Environment]::GetFolderPath($name)){
		$Folder = $path
	}
	else
	{
		Write-Verbose "[Warning] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	return $Folder
}
export-modulemember -function Get-SystemFolder

#-----------------------------------------------------------------------
# Move-Path [-Path [<String>]] [-Destination [<String>]]
# [-Exclude [<String[]>]]
#
# Example: .\Move-Path -Path \\source\path
#-----------------------------------------------------------------------
function Move-Path
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Destination = $(throw '-Destination is a required parameter.'),
		[string]$Exclude = ""
	)
	Write-Verbose "Move-Path -Path $Path -Destination $Destination"
	$Path = Remove-Suffix -String $Path -Remove "\"
	if (test-folder -Path $Path)
	{
		Remove-Path -Destination $Destination
		New-Path -Destination $Destination
		Copy-Recurse -Path $Path -Destination $Destination -Exclude $Exclude
		Remove-Path -Destination $Path
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path moved to -Destination $Destination"
	}
	else
	{
		Write-Verbose "[Error] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."
	}
}
export-modulemember -function Move-Path

#-----------------------------------------------------------------------
# New-Path [-Path [<String>]]
#
# Example: .\New-Path \\source\path
#-----------------------------------------------------------------------
function New-Path
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[bool]$Clean=$false
	)
	Write-Verbose "New-Path -Path $Path"
	[String]$Folder = ""
	$Path = Remove-Suffix -String $Path -Remove "\"	
	if ($Clean) {Remove-Path -Path $Path}
	if (-not (test-path $Path)) {
		if (Test-Unc $Path)
		{
			$PathArray = $Path.Split('\')
			foreach($item in $PathArray)
			{
				if($item.Length -gt 0)
				{
					if($Folder.Length -lt 1)
					{
						$Folder = "\\$item"
					}
					else
					{
						$Folder = "$Folder\$item"
						if (-not (Test-Path $Folder)) {
							New-Item -ItemType directory -Path $Folder -Force
						}
					}
				}
			}
		}
		else
		{
				New-Item -ItemType directory -Path $Path -Force
		}
	}
}
export-modulemember -function New-Path

#-----------------------------------------------------------------------
# Redo-Path [-Path [<String>]] [-Destination [<String>]]
# [-Exclude [<String[]>]]
#
# Example: .\Redo-Path -Path \\source\path
#-----------------------------------------------------------------------
function Redo-Path
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Destination = $(throw '-Destination is a required parameter.'),
		[string]$Exclude = ""		
	)
	Write-Verbose "Redo-Path -Path $Path -Destination $Destination"
	$Path = Remove-Suffix -String $Path -Remove "\"
	Remove-Path -Destination $Path
	New-Path -Destination $Path
	Copy-Recurse -Path $Path -Destination $Destination -Exclude $Exclude
	Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path to -Destination $Destination"
}
export-modulemember -function Redo-Path

#-----------------------------------------------------------------------
# Remove-File [-File [<String>]]
#
# Example: .\Remove-File \\source\path\file.txt
#-----------------------------------------------------------------------
function Remove-File
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path=$(throw '-Path is a required parameter.')
	)
	Write-Verbose "Remove-File -Path $Path"
	if (Test-File -Path $Path) { 
		Remove-Item -Path $Path -Force 		
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path removed."
	}
	else
	{
		Write-Verbose "[Error] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."
	}
}
export-modulemember -function Remove-File

#-----------------------------------------------------------------------
# Remove-Path [-Path [<String>]]
# [-Include [<String[]>] [-Exclude [<String[]>]]
#
# Example: .\Remove-Path -Path \\source\path
#-----------------------------------------------------------------------
function Remove-Path
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[string[]]$Include = "*.*",
		[string[]]$Exclude = "",
		[Int16]$Retention = 1,
		[Int32]$First = 1000
	)
	Write-Verbose "Remove-Path -Path $Path -Include $Include -Exclude $Exclude -Retention $Retention -First $First"
	$Path = Remove-Suffix -String $Path -Remove "\"
	if (test-folder -Path $Path) {
		$ErrorActionPreferenceBackup = $ErrorActionPreference
		$ErrorActionPreference = 'SilentlyContinue'
		Get-ChildItem -Path $Path -Include $Include -Exclude $Exclude -Recurse | Where-Object {($_.PSIsContainer) -and ($_.lastwritetime -le (get-date).addDays(($Retention*-1)))} | select -First $First | Remove-Item -Force -Recurse
		Remove-Item $Path -Recurse -Force
		$ErrorActionPreference = $ErrorActionPreferenceBackup
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path removed."
	}
	else
	{
		Write-Verbose "[Warning] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
}
export-modulemember -function Remove-Path

#-----------------------------------------------------------------------
# Remove-Subfolders [-Path [<String>]]
# [-Include [<String[]>] [-Exclude [<String[]>]]
#
# Example: .\Remove-Subfolders -Path \\source\path
#-----------------------------------------------------------------------
function Remove-Subfolders
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Subfolder = $(throw '-Subfolder is a required parameter.'),
		[Int32]$First = 1000
	)
	Write-Verbose "Remove-Subfolders -Path $Path -Subfolder $Subfolder -First $First"
	$Path = Set-Unc -Path $Path
	
	if (test-folder -Path $Path) {
		$Affected = 0
		if(Test-Unc -Path $Path)
		{
			$Folders=Get-ChildItem $Path -Recurse | Where-Object {($_.Name -EQ $Subfolder) -and ($_.PSIsContainer)} | select -First $First
			foreach ($Folder in $Folders) {
				if ($Folder.FullName)
				{
					[String]$FolderToRemove=Add-Suffix -String $Folder.FullName -Add "\"
					Remove-Path -Path $FolderToRemove
					$Affected = $Affected + 1
				}
			}
		}
		else
		{
			$Remove = Add-Suffix -String $Path -Add '\'
			$Remove = Add-Suffix -String $Remove -Add $Subfolder
			Remove-Path -Path $Remove
			$Affected = 1
		}
		Write-Verbose "[Success] $Affected items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path -Subfolder $Subfolder removed."
	}
	else
	{
		Write-Verbose "[Warning] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."
	}
}
export-modulemember -function Remove-Subfolders

#-----------------------------------------------------------------------
# Remove-Recurse [-Source [<String>]] [-Destination [<String>]]
# [-Include [<String[]>] [-Exclude [<String[]>]]
#
# Example: .\Remove-Recurse \\source\path \\destination\path
#-----------------------------------------------------------------------
function Remove-Recurse
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Source is a required parameter.'),
		[string[]]$Include = "*.*",
		[string[]]$Exclude = "",
		[Int32]$First = 1000
	)
	Write-Verbose "Remove-Recurse -Path $Path -Include $Include -Exclude $Exclude"	
	$Path = Remove-Suffix -String $Path -Remove "\"		
	if (Test-Path $Path)
	{
		$PathAbsolute = Convert-PathSafe -Path $Path
		$Items = Get-ChildItem -Path $PathAbsolute -Recurse -Include $Include -Exclude $Exclude | where { ! $_.PSIsContainer }
		$Affected = 0
		ForEach ($Item in $Items) {
			Remove-File -Path $Item
			$Affected = $Affected + 1
		}
		Write-Verbose "[Success] $Affected items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path."
	}
	else
	{
		Write-Verbose "[Warning] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."
	}
}
export-modulemember -function Remove-Recurse

#-----------------------------------------------------------------------
# Remove-Prefix [-String [<String>]]
#
# Example: .\Remove-Prefix -String Hell -Remove o
#	Result: Hello
#-----------------------------------------------------------------------
function Remove-Prefix
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$String = $(throw '-String is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Remove = $(throw '-Remove is a required parameter.')
	)
	Write-Verbose "Remove-Prefix -String $String -Remove $Remove"
	[string]$ReturnValue = $String
	if (Compare-IsFirst -String $String -BeginsWith $Remove)
	{ 		
		$ReturnValue = $String.Substring($Remove.Length, $String.Length - $Remove.Length)
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -String $String already has prefix of -Remove $Remove"
	}

	return $ReturnValue
}
export-modulemember -function Remove-Prefix

#-----------------------------------------------------------------------
# Remove-Suffix [-String [<String>]]
#
# Example: .\Remove-Suffix -String Hell -Remove o
#	Result: Hello
#-----------------------------------------------------------------------
function Remove-Suffix
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$String = $(throw '-String is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Remove = $(throw '-Remove is a required parameter.')
	)
	Write-Verbose "Remove-Suffix -String $String -Remove $Remove"
	[string]$ReturnValue = $String
	if($String)
	{
		if (Compare-IsLast -String $String -EndsWith $Remove)
		{ 		
			$ReturnValue = $ReturnValue.Substring(0, $String.Length - $Remove.Length)
		}
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -String $String already has suffix of -Remove $Remove"
	}

	return $ReturnValue
}
export-modulemember -function Remove-Suffix

#-----------------------------------------------------------------------
# Remove-Element [-Path [<String>]] 
#
# Example: .\Remove-Element -Value "<RootNode><ChildNode><Element1 /></ChildNode></RootNode>" 
#		-XPath "//msb:None/msb:Generator"
#		-Namespace @{msb = "http://schemas.microsoft.com/developer/msbuild/2003"}
#
# Called: $XMLValue = [xml](Get-Content $path)
#		$Namespace = @{msb = 'http://schemas.microsoft.com/developer/msbuild/2003'}
#		Remove-Element $XMLValue -XPath '//msb:None/msb:Generator' -Namespace $Namespace
#		$proj.Save($path)
#-----------------------------------------------------------------------
function Remove-Element
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[xml]$Value=$(throw '-Value is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$XPath=$(throw '-Value is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[String]$Namespace=$(throw '-Value is a required parameter.')		
	)
	Write-Verbose ".\Remove-Element -Value $Value -XPath $XPath -Namespace $Namespace -SingleNode"
    $nodes = @(Select-Xml $XPath $Value -Namespace $Namespace | Foreach {$_.Node})
    if (!$nodes) { Write-Verbose "RemoveElement: XPath $XPath not found" }
    if ($singleNode -and ($nodes.Count -gt 1)) {
        throw "XPath $XPath found multiple nodes" 
    }
	$Count = 0
    foreach ($node in $nodes)
	{
        $parentNode = $node.ParentNode
        [void]$parentNode.RemoveChild($node)
		$Count = $Count + 1
    }
	Write-Verbose "[Success] $Count items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
}
export-modulemember -function Remove-Element

#-----------------------------------------------------------------------
# Remove-ContentsByTagContains [-Path [<String>]]
# [-Open [<String[]>] [-Close [<String[]>]]
#
# Example: .\Remove-ContentsByTagContains \\source\path \\destination\path
#	GlobalSection(TeamFoundationVersionControl) = preSolution
#	EndGlobalSection
#-----------------------------------------------------------------------
function Remove-ContentsByTag
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Open = $(throw '-Open is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Close = $(throw '-Close is a required parameter.'),
		[string[]]$Include = "*.*",
		[string[]]$Exclude = "",
		[Int32]$First = 100
	)
	Write-Verbose "Remove-ContentsByTag -Path $Path -Open $Open -Close $Close -Include $Include -Exclude $Exclude -First $First"
	$Path = Remove-Suffix -String $Path -Remove "\"
	if (Test-Path $Path)
	{
		$Open = $Open.Trim()
		$Close = $Close.Trim()
		$Files = Get-Childitem -Path $Path -Include $Include -Exclude $Exclude -Recurse -Force | select -First $First
		ForEach ($File in $Files)
		{		
			[Int32]$OpenIndex = -1
			[Int32]$CloseIndex = -1
			$Content=Get-Content $File.PSPath
			# Search for matches
			For([Int32]$Count = 0; $Count -lt $Content.Length; $Count++)
			{
				$CurrentLine = $Content[$Count].Trim()
				If(($OpenIndex -eq -1) -and ($CurrentLine -eq $Open))
				{
					$OpenIndex = $Count
				}
				ElseIf(($OpenIndex -gt -1) -and ($CurrentLine -eq $Close))
				{
					$CloseIndex = $Count
					Break
				}
			}
			# Evaluate search
			If(($OpenIndex -gt -1) -and ($OpenIndex -lt $CloseIndex))
			{			
				# Match Found Remove block.
				$NewContent = ($Content | Select -First $OpenIndex) + ($Content | select -Last ($Content.Length - $CloseIndex - 1))
			}
			else
			{
				# No Match Found
				$NewContent = $Content
			}
			Set-Content $File.PSPath -Value $NewContent
		}
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
}
export-modulemember -function Remove-ContentsByTag

#-----------------------------------------------------------------------
# Remove-ContentsByTagContains [-Path [<String>]]
# [-Open [<String[]>] [-Close [<String[]>]]
#
# Example: .\Remove-ContentsByTagContains \\source\path \\destination\path
#	GlobalSection(TeamFoundationVersionControl) = preSolution
#	EndGlobalSection
#-----------------------------------------------------------------------
function Remove-ContentsByTagContains
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Open = $(throw '-Open is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Close = $(throw '-Close is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Contains = $(throw '-Contains is a required parameter.'),
		[string[]]$Include = "*.*",
		[string[]]$Exclude = "",
		[Int32]$First = 100
	)
	Write-Verbose "Remove-ContentsByTagContains -Path $Path -Open $Open -Close $Close -Contains $Contains -Include $Include -Exclude $Exclude -First $First"
	$Path = Remove-Suffix -String $Path -Remove "\"
	if (Test-Path $Path)
	{
		$Open = $Open.Trim()
		$Close = $Close.Trim()
		$Contains = $Contains.Trim()

		$Files = Get-Childitem -Path $Path -Include $Include -Exclude $Exclude -Recurse -Force | select -First $First	
		ForEach ($File in $Files)
		{		
			[Int32]$OpenIndex = -1
			[Int32]$ContainsIndex = -1
			[Int32]$CloseIndex = -1
			$Content=Get-Content $File.PSPath
			# Search for matches
			For([Int32]$Count = 0; $Count -lt $Content.Length; $Count++)
			{
				$CurrentLine = $Content[$Count].Trim()			
				If ($CurrentLine -like "*$Open*")
				{				
					If($OpenIndex -gt -1)
					{
						# Fail: Block did not contain -Content and/or -Open was found before -Close. Reset for next open tag match.
						$ContainsIndex = -1
						$CloseIndex = -1
					}
					$OpenIndex = $Count
				}ElseIf($OpenIndex -gt -1)
				{
					If($CurrentLine -like "*$Contains*")
					{
						$ContainsIndex = $Count
					}ElseIf(($ContainsIndex -gt -1) -and ($CurrentLine -like "*$Close*"))
					{
						# Success, block starts with -Open, ends with -Close and includes -Contains
						$CloseIndex = $Count
						Break
					}
				}
			}
			# Any matches?
			If(($OpenIndex -gt -1) -and ($ContainsIndex -gt $OpenIndex) -and ($CloseIndex -gt $ContainsIndex))
			{			
				If($CloseIndex -eq ($OpenIndex + 2))
				{
					# Match Found with single element. Remove Block.
					$NewContent = ($Content | Select -First $OpenIndex) + ($Content | select -Last ($Content.Length - $CloseIndex - 1))
				}
				Else
				{			
					# Match Found with multiple elements. Remove Line Only.
					$NewContent = ($Content | Select -First $ContainsIndex) + ($Content | select -Last ($Content.Length - $ContainsIndex -1))
				}					
			}
			else
			{
				# No Match Found
				$NewContent = $Content
			}
			Set-Content $File.PSPath -Value $NewContent
		}
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
}
export-modulemember -function Remove-ContentsByTagContains

#-----------------------------------------------------------------------
# Rename-File [-File [<String>]]
#
# Example: .\Rename-File -Path ($StagingZipPath + 'root.vstbak') -NewName root.vstemplate -Force
#-----------------------------------------------------------------------
function Rename-File
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path=$(throw '-Path is a required parameter.'),
		[string]$NewName=$(throw '-NewName is a required parameter.')
	)
	Write-Verbose "Rename-File -Path $Path -NewName $NewName"
	if (Test-File -Path $Path) {
		Rename-Item -Path $Path -NewName $NewName -Force
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path removed."
	}
	else
	{
		Write-Verbose "[Error] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."
	}
}
export-modulemember -function Rename-File

#-----------------------------------------------------------------------
# Set-ReadOnly [-Path [<String>]] [-ReadOnly [<bool>]]
#
# Example: .\Set-ReadOnly -Path \\source\path\File.name -ReadOnly $False
#-----------------------------------------------------------------------
function Set-ReadOnly
{
	param (
		[Parameter(Mandatory = $True)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[bool]$ReadOnly = $True,
		[string]$ErrorPreference = 'SilentlyContinue'
	)
	Write-Verbose "Set-ReadOnly -Path $Path -ReadOnly $ReadOnly -ErrorPreference $ErrorPreference"
	$Path = Remove-Suffix -String $Path -Remove "\"
	if(test-path $Path)
	{
		$PathAbsolute = Convert-PathSafe -Path $Path	
		if (Test-Path $PathAbsolute -PathType Leaf)
		{
			$ErrorActionPreferenceBackup = $ErrorActionPreference
			$ErrorActionPreference = $ErrorPreference
			Set-ItemProperty $PathAbsolute -name IsReadOnly -value $ReadOnly -Force
			$ErrorActionPreference = $ErrorActionPreferenceBackup
		}
		Write-Verbose "[Success] 1 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path set."
	}
	else
	{
		Write-Verbose "[Error] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."
	}
}
export-modulemember -function Set-ReadOnly

#-----------------------------------------------------------------------
# Set-SystemFolderDrives
#
# Example: Set-SystemFolderDrives
#-----------------------------------------------------------------------
function Set-SystemFolderDrives
{
	param (
	)
	Write-Verbose "Set-SystemFolderDrives"

	$SpecialFolders = @{}
	$names = [Environment+SpecialFolder]::GetNames([Environment+SpecialFolder])
	foreach($name in $names)
	{
	  if($path = [Environment]::GetFolderPath($name)){
		$SpecialFolders[$name] = $path
 		New-PSDrive -Name $name -PSProvider FileSystem -Root $path
	  }
	}
	#	#TBD: Find the 10 Largest Files in the Documents Folder
	#	gci Personal: -Recurse -Force -ea SilentlyContinue |
	#	   Sort-Object -Property Length -Descending |
	#	   Select-Object -First 10 |
	#	   Format-Table -AutoSize -Wrap -Property `
	# 	 Length,LastWriteTime,FullName
	return $SpecialFolders
}
export-modulemember -function Set-SystemFolderDrives

#-----------------------------------------------------------------------
# Test-File [-Path [<String>]]
#
# Example: .\Test-File -Path \\source\path
#-----------------------------------------------------------------------
function Test-File
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.')
	)
	Write-Verbose "Test-File -Path $Path"
	[bool]$ReturnValue = $false
	if(Test-Path -Path $Path -PathType Leaf)
	{
		$ReturnValue = $true
	}
	else
	{
		Write-Verbose "[Warning] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist or is not a File."
	}
	return $ReturnValue
}
export-modulemember -function Test-File

#-----------------------------------------------------------------------
# Test-Folder [-Path [<String>]]
#
#
# Example: .\Test-Folder -Path \\source\path
#-----------------------------------------------------------------------
function Test-Folder
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.')
	)
	Write-Verbose "Test-Folder -Path $Path"
	[bool]$ReturnValue = $false
	if(test-path -Path $Path -PathType Container)
	{
		$ReturnValue = $true
	}	
	else
	{
		Write-Verbose "[Warning] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist or is not a Folder."
	}
	return $ReturnValue
}
export-modulemember -function Test-Folder

#-----------------------------------------------------------------------
# Test-PathEmpty [-Path [<String>]]
#
# Example: .\Test-PathEmpty -Path \\source\path
#-----------------------------------------------------------------------
function Test-PathEmpty
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.')
	)
	Write-Verbose "Test-PathEmpty -Path $Path"
	[bool]$ReturnValue = $false
	if((Get-ChildItem $Path -force | Select-Object -First 1 | Measure-Object).Count -eq 0)
	{
		$ReturnValue = $true
	}
	else
	{
		Write-Verbose "[Error] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path does not exist."
	}
	return $ReturnValue
}
export-modulemember -function Test-PathEmpty

#-----------------------------------------------------------------------
# Set-Unc [-Path [<String>]]
#
# Example: .\Set-Unc -Path \\source\path
#-----------------------------------------------------------------------
function Set-Unc
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.')
	)
	Write-Verbose "Set-Unc -Path $Path"
	$Path = $Path.Trim()
	$Path = Remove-Suffix -String $Path -Remove '\'
	if(-not ($Path.Contains(':\') -or $Path.Contains('.\') -or (Compare-IsFirst -String $Path -BeginsWith '\')))
	{
		$ReturnValue = Add-Prefix -String $Path -Add '\\'
	}
	else
	{
		$ReturnValue = $Path
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine). -Path $Path already a UNC, drive letter, absolute or relative path."
	}
	return $ReturnValue
}
export-modulemember -function Set-Unc

#-----------------------------------------------------------------------
# Test-Unc [-Path [<String>]]
#
#
# Example: .\Test-Unc -Path \\source\path
#-----------------------------------------------------------------------
function Test-Unc
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.')
	)
	Write-Verbose "Test-Unc -Path $Path"
	[bool]$ReturnValue = $false
	if(($Path.Contains('\\')) -and (-not ($Path.Contains(':\'))))
	{
		$ReturnValue = $true
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	return $ReturnValue
}
export-modulemember -function Test-Unc

#-----------------------------------------------------------------------
# Update-LineByContains [-Path [<String>]]
# [-Contains [<String[]>] [-Close [<String[]>]]
#
# Example: .\Update-LineByContains -Path \\source\path -Include AssemblyInfo.cs -Contains 'AssemblyVersion(' -Line '[assembly: AssemblyVersion("5.20.07")]'
#-----------------------------------------------------------------------
function Update-LineByContains
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Contains = $(throw '-Contains is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Line = $(throw '-Line is a required parameter.'),
		[string[]]$Include = "*.*",
		[string[]]$Exclude = "",
		[Int32]$First = 100
	)
	Write-Verbose "Update-LineByContains -Path $Path -Contains $Contains -Line $Line -Include $Include -Exclude $Exclude -First $First"
	$Path = Remove-Suffix -String $Path -Remove "\"
	if (Test-Path $Path)
	{
		$Contains = $Contains.Trim()
		$Count = 0
		$Files = Get-Childitem -Path $Path -Include $Include -Exclude $Exclude -Recurse -Force | select -First $First
		ForEach ($File in $Files)
		{
			[Int32]$ContainsIndex = -1
			$Affected = 0
			$Content=Get-Content $File.PSPath
			# Search for matches
			For([Int32]$Count = 0; $Count -lt $Content.Length; $Count++)
			{
				$CurrentLine = $Content[$Count].Trim()
				If(($ContainsIndex -eq -1) -and ($CurrentLine -eq $Contains))
				{
					$ContainsIndex = $Count
					Break
				}
			}
			# Evaluate search
			If($ContainsIndex -gt -1)
			{			
				# Select before line, add -Line, select after line
				$NewContent = (($Content | Select -First $ContainsIndex) + ($Line + [Environment]::NewLine) + ($Content | select -Last ($Content.Length - $ContainsIndex -1)))
			}
			else
			{
				# No Match Found
				$NewContent = $Content
			}
			Set-Content $File.PSPath -Value $NewContent
			$Affected = $Count
		}
		Write-Verbose "[Success] $Count items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
}
export-modulemember -function Update-LineByContains

#-----------------------------------------------------------------------
# Update-ContentsByTag [-Path [<String>]]
# [-Open [<String[]>] [-Close [<String[]>]]
#
# Example: .\Update-ContentsByTag -Path $Path -Include *.sln -Open "GlobalSection(TeamFoundationVersionControl) = preSolution" -Close "EndGlobalSection"
#-----------------------------------------------------------------------
function Update-ContentsByTag
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Open = $(throw '-Open is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Close = $(throw '-Close is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Value = $(throw '-Value is a required parameter.'),
		[string[]]$Include = "*.*",
		[string[]]$Exclude = "",
		[Int32]$First = 100
	)
	Write-Verbose "Update-ContentsByTag -Path $Path -Open $Open -Close $Close -Include $Include -Exclude $Exclude -First $First"
	$Path = Remove-Suffix -String $Path -Remove "\"
	[String]$PaddingLeft = '    '
	if (Test-Path $Path)
	{
		$Open = $Open.Trim()
		$Close = $Close.Trim()
		$Affected = 0
		$Files = Get-Childitem -Path $Path -Include $Include -Exclude $Exclude -Recurse -Force | select -First $First
		ForEach ($File in $Files)
		{
			[Int32]$OpenIndex = -1
			[Int32]$CloseIndex = -1
			[String]$NewValue = ""
			$Content=Get-Content $File.PSPath
			# Search for matches
			For([Int32]$Count = 0; $Count -lt $Content.Length; $Count++)
			{
				$CurrentLine = $Content[$Count].Trim()
				If(($OpenIndex -eq -1) -and ($CurrentLine -like "*$Open*"))
				{
					$OpenIndex = $Count
				}
				If(($OpenIndex -gt -1) -and ($CurrentLine -like "*$Close*"))
				{
					$CloseIndex = $Count
					Break
				}
			}
			# Evaluate search
			If(($OpenIndex -gt -1) -and ($OpenIndex -le $CloseIndex))
			{				
				if($OpenIndex -eq $CloseIndex)
				{
					# Open/Close on same line, rebuild the line with new contents
					$NewValue = ($Open + $Value + $Close)
				}
				else
				{
					$NewValue = $Value
				}
				# Update content
				$NewContent = ($Content | Select -First ($OpenIndex)) + ($PaddingLeft + $NewValue) + ($Content | select -Last ($Content.Length - $CloseIndex - 1))
				$Affected = 1
			}
			else
			{
				# No Match Found
				$NewContent = $Content
			}
			Set-Content $File.PSPath -Value $NewContent
		}
		Write-Verbose "[Success] $Affected items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
}
export-modulemember -function Update-ContentsByTag
	
#-----------------------------------------------------------------------
# Update-Text [-Path [<String>]]
# [-Include [<String[]>] [-Exclude [<String[]>]]
#
# Example: .\Update-Text -Path \\source\path -Include *.cs -Old "Use gotos" -New "Point at people who use gotos"
#-----------------------------------------------------------------------
function Update-Text
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Old = $(throw '-Old is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$New = $(throw '-New is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string[]]$Include = $(throw '-Include is a required parameter.'),
		[string[]]$Exclude = "",
		[Int32]$First = 100
	)
	Write-Verbose "Update-Text -Path $Path -Old $Old -New $New -Include $Include -Exclude $Exclude -First $First"
	$Path = Remove-Suffix -String $Path -Remove "\"
	$Count = 0
	if (Test-Path $Path)
	{
		$ConfigFiles=Get-Childitem  $Path -Include $Include -Exclude $Exclude -Recurse -Force | select -First $First
		foreach ($Item in $ConfigFiles)
		{
			Set-ReadOnly -Path $Item.PSPath -ReadOnly $false
			(Get-Content $Item.PSPath) | 
			Foreach-Object {$_.Replace($Old, $New)
			} | 
			Set-Content $Item.PSPath -force
			$Count = $Count + 1
		}
		Write-Verbose "[Success] $Count items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
}
export-modulemember -function Update-Text

#-----------------------------------------------------------------------
# Update-TextByContains [-Path [<String>]]
# [-Contains [<String[]>] [-Close [<String[]>]]
#
# Example: .\Update-TextByContains -Path \\source\path -Include AssemblyInfo.cs -Contains 'AssemblyVersion(' -Line '[assembly: AssemblyVersion("5.20.07")]'
#-----------------------------------------------------------------------
function Update-TextByContains
{
	param (		
		[string]$Path = $(throw '-Path is a required parameter.'),
		[string]$Contains = $(throw '-Contains is a required parameter.'),
		[string]$Old = $(throw '-Old is a required parameter.'),
		[string]$New = $(throw '-New is a required parameter.'),
		[string[]]$Include = "*.*",
		[string[]]$Exclude = "",
		[Int32]$First = 100
	)
	Write-Verbose "Update-TextByContains -Path $Path -Contains $Contains -Old $Old -New $New -Include $Include -Exclude $Exclude -First $First"
	$Path = Remove-Suffix -String $Path -Remove "\"
	if (Test-Path $Path)
	{
		$Contains = $Contains.Trim()
		$Count = 0
		$Files = Get-Childitem -Path $Path -Include $Include -Exclude $Exclude -Recurse -Force | select -First $First
		ForEach ($File in $Files)
		{
			[Int32]$FoundIndex = -1
			[String]$FoundLine = ''
			$Affected = 0
			$Content=Get-Content $File.PSPath
			# Search for matches
			For([Int32]$Count = 0; $Count -lt $Content.Length; $Count++)
			{
				$CurrentLine = $Content[$Count].Trim()
				If(($FoundIndex -eq -1) -and ($CurrentLine.ToLowerInvariant().Contains($Contains.ToLowerInvariant())))
				{
					$FoundIndex = $Count
					$FoundLine = $CurrentLine
					Break
				}
			}
			# Evaluate search
			If($FoundIndex -gt -1)
			{			
			    # Replace text inside of line
				$NewLine = $FoundLine.Replace($Old, $New)
				# Select before line, add $NewLine, select after line
				$NewContent = (($Content | Select -First $FoundIndex) + ($NewLine + [Environment]::NewLine) + ($Content | select -Last ($Content.Length - $FoundIndex -1)))
			}
			else
			{
				# No Match Found
				$NewContent = $Content
			}
			Set-Content $File.PSPath -Value $NewContent
			$Affected = $Count
		}
		Write-Verbose "[Success] $Count items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
}
export-modulemember -function Update-TextByContains

#-----------------------------------------------------------------------
# Update-TextByTable [-Path [<String>]] [-Replace [<hashtable>]]
# [-Include [<String[]>] [-Exclude [<String[]>]]
#
# Example: .\Update-TextByTable -Path \\source\path -Include *.cs
#				-Replace @{'Old1' = 'New1'
#							'Old2' = 'New2'
#							'Old3' = 'New3'}
#-----------------------------------------------------------------------
function Update-TextByTable
{
	param (
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string]$Path = $(throw '-Path is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[hashtable]$Replace = $(throw '-Replace is a required parameter.'),
		[Parameter(Mandatory=$true,ValueFromPipelineByPropertyName=$true)]
		[string[]]$Include = $(throw '-Include is a required parameter.'),
		[string[]]$Exclude = "",
		[Int32]$First = 100
	)
	Write-Verbose "Update-Text -Path $Path -Old $Old -New $New -Include $Include -Exclude $Exclude -First $First"
	$Path = Remove-Suffix -String $Path -Remove "\"
	if (Test-Path $Path)
	{
		Write-Verbose "Get-Childitem  $Path -Include $Include -Exclude $Exclude -Recurse -Force | select -First $First"
		$ConfigFiles=Get-Childitem  $Path -Include $Include -Exclude $Exclude -Recurse -Force | select -First $First
		Write-Verbose "ConfigFiles: $ConfigFiles"
		$Count = 0
		foreach ($Item in $ConfigFiles)
		{
			Write-Verbose "Get-Content $Item.PSPath"
			$fileLines = Get-Content $Item.PSPath
			Write-Verbose "fileLines: $fileLines"
			if($fileLines)
			{
				foreach($replaceItem in $Replace.GetEnumerator()) {
					Write-Verbose "$fileLines.Replace($replaceItem.Key, $replaceItem.Value)"
					$fileLines = $fileLines.Replace($replaceItem.Key, $replaceItem.Value)
				}
				Write-Verbose "Set-Content -Path $Item.PSPath -Value $fileLines -force"
				Set-Content -Path $Item.PSPath -Value $fileLines -force
			}
			$Count = $Count + 1
		}
		Write-Verbose "[Success] $Count items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
	else
	{
		Write-Verbose "[OK] 0 items affected. $(Get-CurrentFile) at $(Get-CurrentLine)."
	}
}
export-modulemember -function Update-TextByTable