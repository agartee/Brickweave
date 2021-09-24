# Basic Messaging Demo

## Generating additional EF Migrations

From the root folder, run this command, replacing the `$migrationName` with a real value:

```powershell
dotnet ef migrations add $migrationName --startup-project ".\samples\2. BasicMessaging\src\BasicMessaging.WebApp" --project ".\samples\2. BasicMessaging\src\BasicMessaging.SqlServer"
```

## Deploying a specific EF Migration

To make executing migrations easier, a helper PowerShell script can be found in the `./script` folder. Migrations may also be run using the following command.

```powershell
dotnet ef database update $migrationName --startup-project ./src/ReporterCore.WebApp/ --project ./src/ReporterCore.SqlServer/
```
