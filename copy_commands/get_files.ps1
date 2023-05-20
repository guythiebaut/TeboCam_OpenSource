#//https://linuxhint.com/output-file-powershell/#1

$commandFile = "C:\Dropbox\projects\TeboCam_OpenSource\copy_commands\copyCommands.bat";
$SourceDirectory =  "C:\Dropbox\projects\TeboCam\TeboCam";
$DestinationDirectory = "C:\Dropbox\projects\TeboCam_OpenSource\Tebocam";
$SupplementalSourceDirectory =  "C:\Dropbox\projects\TeboCam";
$SupplementalDestinationDirectory = "C:\Dropbox\projects\TeboCam_OpenSource";

class FileInformation {
	[String] $FileName
	[String] $SourceDirectory
	[String] $DestinationDirectory
	[Boolean] $Include

	FileInformation([string] $fileName, [string] $directory, [string] $destinationDirectory, [Boolean] $include) {
		$this.FileName = $fileName;
		$this.SourceDirectory = $directory;
		$this.DestinationDirectory = $destinationDirectory;
		$this.Include = $include;
	}
}	

$fileCollection = New-Object System.Collections.Generic.List[FileInformation];
$SuplementalItemsArray = @("README.md", "TeboCam.sln", "TeboCam.suo","TeboCam.v12.suo"); 

function WaitForKeypress {
	$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
}


function ExcludedDirs {
	return @("C:\Dropbox\projects\TeboCam\TeboCam\bin", 
			 "C:\Dropbox\projects\TeboCam\TeboCam\obj"
  			);
}

function ExcludedFiles {
	return @("sensitiveInfo.cs");
}

function BuildLine {
	param($dirString, $destDirString, $fileString);
	$from = $dirString + "\" + $fileString;
	$to = $destDirString + "\" + $fileString;
	$line = "copy ""$from"" ""$to""";
	return $line;
}

function AddSuplementaryLines {

	foreach ($item in $SuplementalItemsArray)
	{
		$item = [FileInformation]::new($item, $SupplementalSourceDirectory, $SupplementalDestinationDirectory, $true);
		$fileCollection.add($item);
	}
}

function ExcludeItem {
	param($inDirectory, $excludeArray);
	
	foreach ( $node in $excludeArray )
	{
		$matched = IsMatch $inDirectory $node;
		
		if ($matched) { 
			return $true;
		}
	}	
	
	return $false;
}

function IsMatch {
	param($masterString, $stringToMatch);
	$matched = "$masterString" -like "*$stringToMatch*";
	return $matched;
}


function FilesToArray {
	Write-Host $fileCollection;
	WaitForKeypress;
	$files = Get-ChildItem "C:\Dropbox\projects\TeboCam\TeboCam\" -Recurse;

	foreach ($f in $files) {
		$fullName = $f.FullName;
		$fileName = $f.Name;
		$directoryName = $f.DirectoryName;
		$excludedDirs = ExcludedDirs;
		$excludedFiles = ExcludedFiles;
		$excludeDirectory =  ExcludeItem $directoryName $excludedDirs 
		$excludeFile =  ExcludeItem $fileName $excludedFiles
		$include = $true;
		
		if ($excludeDirectory -Or $excludeFile -Or -Not $directoryName) {
			$include = $false;
		}
		
		if ($directoryName) {
			$destDir = $directoryName.replace($SourceDirectory, $DestinationDirectory);
		}
		
		$item = [FileInformation]::new($fileName, $directoryName, $destDir, $include);
		$fileCollection.add($item);
	}
}

function ProcessFileArray {

	"" | out-file -Encoding UTF8 $commandFile -Append

	foreach ($item in $fileCollection)
	{
		if ($item.Include -Eq $true) {
			# Write-Host $item.Include;	
			# Write-Host $item.Directory;	
			# Write-Host $item.FileName;	
			CreateDirIfNotExist $item.DestinationDirectory
			$line = BuildLine $item.SourceDirectory $item.DestinationDirectory $item.FileName;
			$line | out-file -Encoding UTF8 $commandFile -Append
		}
	}

	"" | out-file -Encoding UTF8 $commandFile -Append
	"pause" | out-file -Encoding UTF8 $commandFile -Append
}

function CreateDirIfNotExist {
	param($dirToCreate);	

	If(!(test-path -PathType container $dirToCreate))
	{
		New-Item -ItemType Directory -Path $dirToCreate
	}
}

function SetUp {
	if (test-path $commandFile) {
		Remove-Item $commandFile;
	}
}


Write-Host "Getting files...";
SetUp;
FilesToArray;
AddSuplementaryLines;
ProcessFileArray;
# Write-Host "Press any key...";
# WaitForKeypress;
