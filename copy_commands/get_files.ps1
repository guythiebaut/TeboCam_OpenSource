class FileInformation {
	[String] $FileName
	[String] $Directory
	[Boolean] $Include

	FileInformation([string] $fileName, [string] $directory, [Boolean] $include) {
		$this.FileName = $fileName;
		$this.Directory = $directory;
		$this.Include = $include;
	}
}	



function WaitForKeypress {
	$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
}

#function ExcludeFiles {
#}

function FilesToArray {
	$fileCollection = New-Object System.Collections.Generic.List[FileInformation];
	Write-Host $fileCollection;
	WaitForKeypress;
	$files = Get-ChildItem "E:\Dropbox\projects\TeboCam\TeboCam\";

	foreach ($f in $files) {
		$fullName = $f.FullName;
		$fileName = $f.Name;
		$directoryName = $f.DirectoryName;
		##$item = [FileInformaton]::new($fileName, $directoryName, $true);
		$item = [FileInformation]::new("1", "2", $true);
		$fileCollection.add($item);
		#Get-Content $f.FullName | Where-Object { ($_ -match 'step4' -or $_ -match 'step9') } | Set-Content $outfile;
		Write-Host $directoryName
		Write-Host $fileName;
	}
}





Write-Host "Getting files...";
FilesToArray;
WaitForKeypress;
