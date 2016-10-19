$env:ASPNETCORE_ENVIRONMENT = "Development"
New-Item -ItemType Directory -Force -Path $env:LOCALAPPDATA\GMBuddy\Databases | Out-Null

Get-ChildItem . -Recurse | Where-Object { $_.PSIsContainer } | Where-Object { $_.Name -match "Migrations" } | Remove-Item -Recurse
Get-ChildItem $env:LOCALAPPDATA\GMBuddy -Recurse | Where-Object { ! $_.PSIsContainer } | Where-Object { $_.Name -match ".*\.sqlite" } | Remove-Item

# create migrations and update databases

cd .\src\GMBuddy.Games
dotnet ef migrations add InitialMigration -c GMBuddy.Games.Dnd35.Data.Dnd35DataContext -o .\Dnd35\Data\Migrations
dotnet ef database update

cd ..\GMBuddy.Identity
dotnet ef migrations add InitialMigration -c GMBuddy.Identity.Data.IdentityContext -o .\Data\Migrations
dotnet ef database update

cd ..\..