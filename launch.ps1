$env:ASPNETCORE_ENVIRONMENT = "Development"

dotnet restore
Start-Process -FilePath "dotnet.exe" -ArgumentList "run" -WorkingDirectory "$pwd\src\GMBuddyIdentity"
Start-Process -FilePath "dotnet.exe" -ArgumentList "run" -WorkingDirectory "$pwd\src\GMBuddyRest"
Start-Process -FilePath "dotnet.exe" -ArgumentList "run" -WorkingDirectory "$pwd\src\GMBuddyData"