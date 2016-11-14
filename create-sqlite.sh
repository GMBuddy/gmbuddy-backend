#!/bin/bash

export ASPNETCORE_ENVIRONMENT=Development
mkdir -p $HOME/.gmbuddy/databases

rm -f $HOME/.gmbuddy/databases/*.sqlite
rm -rf $(find -name Migrations)

# DnD 3.5
(cd ./src/GMBuddy.Games && dotnet ef migrations add InitialMigration -c GMBuddy.Games.Dnd35.Data.Dnd35DataContext -o ./Dnd35/Data/Migrations)
(cd ./src/GMBuddy.Games && dotnet ef database update -c GMBuddy.Games.Dnd35.Data.Dnd35DataContext)

# Microlite 20
(cd ./src/GMBuddy.Games && dotnet ef migrations add InitialMigration -c GMBuddy.Games.Micro20.Data.DatabaseContext -o ./Micro20/Data/Migrations)
(cd ./src/GMBuddy.Games && dotnet ef database update -c GMBuddy.Games.Micro20.Data.DatabaseContext)

# Identity
(cd ./src/GMBuddy.Identity && dotnet ef migrations add InitialMigration -c GMBuddy.Identity.Data.IdentityContext -o ./Data/Migrations)
(cd ./src/GMBuddy.Identity && dotnet ef database update)