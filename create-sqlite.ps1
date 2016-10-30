[CmdletBinding()]
Param(
	[switch]$NoGames,
	[switch]$NoIdentity
)

$env:ASPNETCORE_ENVIRONMENT = "Development"
New-Item -ItemType Directory -Force -Path $env:LOCALAPPDATA\GMBuddy\Databases | Out-Null

Get-ChildItem . -Recurse | Where-Object { $_.PSIsContainer } | Where-Object { $_.Name -match "Migrations" } | Remove-Item -Recurse
Get-ChildItem $env:LOCALAPPDATA\GMBuddy -Recurse | Where-Object { ! $_.PSIsContainer } | Where-Object { $_.Name -match ".*\.sqlite" } | Remove-Item

# create migrations and update databases

if ($NoGames -eq $false) 
{
	Start-Process -FilePath "dotnet.exe" `
		-ArgumentList "ef","migrations","add","InitialMigration", `
			"-c","GMBuddy.Games.Dnd35.Data.Dnd35DataContext", `
			"-o",".\Dnd35\Data\Migrations" `
		-NoNewWindow -Wait `
		-WorkingDirectory ".\src\GMBuddy.Games"
	Start-Process -FilePath "dotnet.exe" `
		-ArgumentList "ef","database","update", `
			"-c","GMBuddy.Games.Dnd35.Data.Dnd35DataContext" `
		-NoNewWindow -Wait `
		-WorkingDirectory ".\src\GMBuddy.Games"
	Start-Process -FilePath "dotnet.exe" `
		-ArgumentList "ef","migrations","add","InitialMigration", `
			"-c","GMBuddy.Games.Micro20.Data.Micro20DataContext", `
			"-o",".\Micro20\Data\Migrations" `
		-NoNewWindow -Wait `
		-WorkingDirectory ".\src\GMBuddy.Games"
	Start-Process -FilePath "dotnet.exe" `
		-ArgumentList "ef","database","update", `
			"-c","GMBuddy.Games.Micro20.Data.Micro20DataContext" `
		-NoNewWindow -Wait `
		-WorkingDirectory ".\src\GMBuddy.Games"
}

if ($NoIdentity -eq $false)
{
	Start-Process -FilePath "dotnet.exe" `
		-ArgumentList "ef","migrations","add","InitialMigration", `
			"-c","GMBuddy.Identity.Data.IdentityContext", `
			"-o",".\Data\Migrations" `
		-NoNewWindow -Wait `
		-WorkingDirectory ".\src\GMBuddy.Identity"
	Start-Process -FilePath "dotnet.exe" `
		-ArgumentList "ef","database","update" `
		-NoNewWindow -Wait `
		-WorkingDirectory ".\src\GMBuddy.Identity"
}

