#!/bin/bash

export ASPNETCORE_ENVIRONMENT=Development
mkdir -p $HOME/.gmbuddy/database

rm -f $HOME/.gmbuddy/databases/*.sqlite
rm -rf $(find -name Migrations)

# create migrations and update databases
cd ./src/GMBuddy.Games
dotnet ef migrations add InitialMigration -c GMBuddy.Games.Dnd35.Data.Dnd35DataContext -o ./Dnd35/Data/Migrations
dotnet ef database update


cd ../GMBuddy.Identity
dotnet ef migrations add InitialMigration -c GMBuddy.Identity.Data.IdentityContext -o ./Data/Migrations
dotnet ef database update

# return to solution root
cd ../../