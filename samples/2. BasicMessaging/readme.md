# Brickweave Basic Messaging Demo

## Running the App

### Entity Framework Core Tools

If not already installed, install the EF Core CLI tools.

```powershell
$ dotnet tool install --global dotnet-ef
```

### User Secrets

Add the following user secrets to the `BasicMessaging.WebApp` project:

```json
"connectionStrings": {
    "demo": "server=[your server];database=[your database];user id=[your user];integrated security=true;MultipleActiveResultSets=True;",
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

If intending to run the app from within Visual Studio, for the `BasicMessaging.WebApp` project, navigate to: **Properties** -> **Debug** -> **General** and click **Open debug launch profiles UI**. Within that dialog, ensure the **App URL** setting is set to the following:

```
https://localhost:5001;http://localhost:5000
```

### Running from the Console

To run the app directly from the console, run the following command from this sample app's folder:

```powershell
$ dotnet run --project ./src/BasicMessaging.WebApp/BasicMessaging.WebApp.csproj --urls="https://localhost:5001;http://localhost:5000"
```

## Maintenance

### Generating additional Entity Framework Database Migrations

From this samples (`\samples\2. BasicMessaging`) folder, run this command, replacing the `$migrationName` with a real value:

```powershell
dotnet ef migrations add $migrationName --startup-project ./src/BasicMessaging.WebApp/ --project ./src/BasicMessaging.SqlServer/
```

### Executing Entity Framework Database Migrations

To make executing migrations easier, a helper PowerShell script can be found in the `./script` folder. Migrations may also be run using the following command.

```powershell
dotnet ef database update --startup-project ./src/BasicMessaging.WebApp/ --project ./src/BasicMessaging.SqlServer/
```
