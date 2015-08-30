New-Item -ItemType Directory -Force -Path .\Build\job
 
Copy-Item King.Azure.Imaging.WebJob\bin\Release\*.dll .\Build\job
Copy-Item King.Azure.Imaging.WebJob\bin\Release\*.exe .\Build\job
Copy-Item King.Azure.Imaging.WebJob\bin\Release\*.config .\Build\job

$directorypath = (Get-Item -Path ".\" -Verbose).FullName + '\Build\job'
$zipPath =  (Get-Item -Path ".\" -Verbose).FullName + '\Build\King.Azure.Imaging.zip'

Add-Type -A System.IO.Compression.FileSystem
[IO.Compression.ZipFile]::CreateFromDirectory($directorypath, $zipPath)