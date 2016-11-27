$env:ASPNETCORE_ENVIRONMENT = "Development"

Start-Process -FilePath "dotnet.exe" -ArgumentList "build" -WorkingDirectory "$pwd\src\GMBuddy.Identity" -NoNewWindow -Wait
Start-Process -FilePath "dotnet.exe" -ArgumentList "build" -WorkingDirectory "$pwd\src\GMBuddy.Rest" -NoNewWindow -Wait

Start-Process -FilePath "dotnet.exe" -ArgumentList "run" -WorkingDirectory "$pwd\src\GMBuddy.Identity"
Start-Sleep -Seconds 2 # DO NOT TAKE THIS OUT IT MAKES EVERYTHING BREAK IF IT LEAVES
Start-Process -FilePath "dotnet.exe" -ArgumentList "run" -WorkingDirectory "$pwd\src\GMBuddy.Rest"