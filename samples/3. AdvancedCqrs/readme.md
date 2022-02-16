# Brickweave Advanced CQRS Demo

## Running the App

You will need the following user secrets to run locally.

### User Secrets

```json
"connectionStrings": {
    "demo": "server=[your server];database=[your database];user id=[your user];password=[your password];MultipleActiveResultSets=True;",
  },
    
  "logging": {
    "logLevel": {
      "default": "debug",
      "system": "information",
      "microsoft": "information"
    },
    "console": {
      "includeScopes": true
    },
    "enableSensitiveDataLogging":  "true"
  }
```

### Generating additional Entity Framework Database Migrations

From this samples (`\samples\3. AdvancedCqrs`) folder, run this command, replacing the `$migrationName` with a real value:

```powershell
dotnet ef migrations add $migrationName --startup-project ./src/AdvancedCqrs.WebApp/ --project ./src/AdvancedCqrs.SqlServer/ --context AdvancedCqrsDbContext
```

### Executing Entity Framework Database Migrations

To make executing migrations easier, a helper PowerShell script can be found in the `./script` folder. Migrations may also be run using the following commands.

```powershell
dotnet ef database update --startup-project ./src/AdvancedCqrs.WebApp/ --project ./src/AdvancedCqrs.SqlServer/ --context AdvancedCqrsDbContext
```

### Add Visual Studio Debug Launch Profile
