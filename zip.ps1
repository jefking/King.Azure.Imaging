$directoryRoot = $args[0]
$directoryPath = $directoryRoot + "Build\job"

Write-Host $directoryRoot
Write-Host $directoryPath

New-Item -ItemType Directory -Force -Path $directoryPath

$dllPath = $directoryRoot + "King.Azure.Imaging.WebJob\bin\Release\*.dll"
$exePath = $directoryRoot + "King.Azure.Imaging.WebJob\bin\Release\*.exe"
$configPath = $directoryRoot + "King.Azure.Imaging.WebJob\bin\Release\*.config"

Write-Host $dllPath
Write-Host $exePath
Write-Host $configPath

Copy-Item $dllPath $directoryPath
Copy-Item $exePath $directoryPath
Copy-Item $configPath $directoryPath

$zipPath = $directoryRoot + "Build\King.Azure.Imaging.zip"

Write-Host $zipPath

If (Test-Path $zipPath){
	Remove-Item $zipPath
}

Add-Type -A System.IO.Compression.FileSystem
[IO.Compression.ZipFile]::CreateFromDirectory($directorypath, $zipPath)