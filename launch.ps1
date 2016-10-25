param([switch]$Quick)

$env:ASPNETCORE_ENVIRONMENT = "Development"

if ($Quick -eq $false) {
	dotnet restore
}

Start-Process -FilePath "dotnet.exe" -ArgumentList "run" -WorkingDirectory "$pwd\src\GMBuddy.Identity"
Start-Sleep -Seconds 2 # DO NOT TAKE THIS OUT IT MAKES EVERYTHING BREAK IF IT LEAVES
Start-Process -FilePath "dotnet.exe" -ArgumentList "run" -WorkingDirectory "$pwd\src\GMBuddy.Rest"