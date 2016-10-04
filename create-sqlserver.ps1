sqllocaldb stop GMBuddy
sqllocaldb delete GMBuddy
sqllocaldb create GMBuddy

Get-ChildItem . -Recurse | Where-Object { $_.PSIsContainer } | Where-Object { $_.Name -match "Migrations" } | Remove-Item -Recurse
Get-ChildItem $env:USERPROFILE | Where-Object { ! $_.PSIsContainer } | Where-Object {$_.Name -match "gmbuddy_.*\.[ml]df" } | Remove-Item

# create migrations and update databases
cd .\src\GMBuddyData
dotnet ef migrations add InitialMigration -c GMBuddyData.Data.DND35.GameContext -o .\Data\DND35\Migrations
dotnet ef database update


cd ..\GMBuddyIdentity
dotnet ef migrations add InitialMigration -c GMBuddyIdentity.Data.ApplicationDbContext -o .\src\GMBuddyIdentity\Data\Migrations
dotnet ef database update

cd ..\..\