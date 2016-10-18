# GMBuddy Backend

## Installation

### Pre-requisites
- `dotnet core 1.0.*`
- sqlite command line tools, if you want to inspect the database

### Steps
- From the command line
  - clone project repo
  - cd into src/GMBuddyData, src/GMBuddyIdentity, and src/GMBuddyRest and run `dotnet restore` in each
  - cd back into the solution root and run create-sqlite.ps1 in PowerShell or create-sqlite.sh in Bash
  - cd into each of the three project directories and type `dotnet run`
  - if you get an error trying to dotnet run GMBuddyIdentity, enter the command `$env:ASPNETCORE_ENVIRONMENT="Development"` using PowerShell in that directory, then try again.
- From visual studio
  - open the .sln file in the project root
  - Wait for projects to "restore" (dependency installation). This should happen automatically.
  - Run create-sqlite.ps1 in PowerShell or create-sqlite.sh in Bash
  - right-click on each project, click "properties", and then change the default debug launch profile to the name of the given project (ie. to the option that is not IIS Express)
  - right click on the solution, click properties, and in the startup project tab, select "multiple startup projects" and change each project to "Start" (or "Start without debugging")
  - Click the green play icon titled Start at the top of the screen (if the icon says IIS Express or one of the project names, then you havent done one of the last steps correctly)

## Development
- Whenever you make a change to a model, be sure to add a migration and update the database. To do this incrementally using SQL Server, look at create-sqlserver.ps1 to see the commands for adding migrations and updating the database. If using SQLite, your best bet is to run create-sqlite.ps1/create-sqlite.sh, which will delete and re-create the entire database from scratch using your new models.
- Develop using Git Flow -- most importantly, do *not* push to Master. `feature/<name>` is the preferred naming convention of branches

## Database
Right now, the project is configured to use SQLite, which will work on Windows, macOS, and Linux. Download SQLite command line tools from the SQLite project website to interact with the database

The project is also configured to optionally use SQL Server Express, which can be installed alongside Visual Studio Community or using [this link](https://msdn.microsoft.com/en-us/library/hh510202.aspx). It requires Windows 7 or later. SQLite has proven to be difficult to work with in Entity Framework Core -- specifically, updates to models require database migrations in EF Core, and SQLite (or, at least, Microsoft's SQLite driver) does not support a number of ALTER statements, meaning that every time we made a model change, we had to completely wipe and re-create the database. While the loss of data in this process was not bad, it was annoying to have to do every time we updated a model, which at this point still happens rather frequently. If you want to keep testing data around more frequently and you are on Windows, use SQL Express. We will soon be integrating an environment variable to switch which database is used.

A script, create-sqlserver.ps1, currently handles the much more infrequent process of clobbering and re-creating the SQL Server Express database. It assumes that you store your mdf files in `$env:USERPROFILE`, which is usually C:\Users\username. If this is not the case for you, submit a pull request to fix it.