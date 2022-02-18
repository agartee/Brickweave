# Brickweave Event Sourcing Demo

## Running the App

### Entity Framework Core Tools

If not already installed, install the EF Core CLI tools.

```powershell
$ dotnet tool install --global dotnet-ef
```

### User Secrets

Add the following user secrets to the `EventSourcingDemo.WebApp` project:

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

### Create the Database

Run the EF migration manually or by running the following script (relative to the sample project):

```powershell
$ .\scripts\migrate.ps1
```

### Visual Studio 2022 Debug Launch Profile

If intending to run the app from within Visual Studio, for the `EventSourcingDemo.WebApp` project, navigate to: **Properties** -> **Debug** -> **General** and click **Open debug launch profiles UI**. Within that dialog, ensure the **App URL** setting is set to the following:

```
https://localhost:5001;http://localhost:5000
```

### Running from the Console

To run the app directly from the console, run the following command from this sample app's folder:

```powershell
$ dotnet run --project ./src/EventSourcingDemo.WebApp/EventSourcingDemo.WebApp.csproj --urls="https://localhost:5001;http://localhost:5000"
```

## Maintenance

### Generating additional Entity Framework Database Migrations

From this samples (`\samples\4. EventSourcing`) folder, run this command, replacing the `$migrationName` with a real value:

```powershell
dotnet ef migrations add $migrationName --startup-project ./src/EventSourcingDemo.WebApp/ --project ./src/EventSourcingDemo.SqlServer/ --context EventSourcingDbContext
```
