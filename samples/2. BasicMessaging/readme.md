# Brickweave Basic Messaging Demo

## Running the App

You will need the following user secrets to run locally.

### User Secrets

```json
"connectionStrings": {
    "demo": "server=[your server];database=[your database];user id=[your user];password=[your password];MultipleActiveResultSets=True;",
    "servicebus": "Endpoint=sb://[your service bus].servicebus.windows.net/;SharedAccessKeyName=[your key name];SharedAccessKey=[your access key]"
  },
  
  "messaging": {
    "queue": "[your message bus queue]"
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

## Generating additional Entity Framework Database Migrations

From this samples (`\samples\3. BasicMessaging`) folder, run this command, replacing the `$migrationName` with a real value:

```powershell
dotnet ef migrations add $migrationName --startup-project ./src/BasicMessaging.WebApp/ --project ./src/BasicMessaging.SqlServer/
```

## Executing Entity Framework Database Migrations

To make executing migrations easier, a helper PowerShell script can be found in the `./script` folder. Migrations may also be run using the following command.

```powershell
dotnet ef database update --startup-project ./src/BasicMessaging.WebApp/ --project ./src/BasicMessaging.SqlServer/
```
