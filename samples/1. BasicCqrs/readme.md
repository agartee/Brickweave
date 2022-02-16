# Brickweave Basic CQRS Demo

## Running the App

### Visual Studio 2022 Debug Launch Profile

If intending to run the app from within Visual Studio, for the `BasicCqrs.WebApp` project, navigate to: **Properties** -> **Debug** -> **General** and click **Open debug launch profiles UI**. Within that dialog, ensure the **App URL** setting is set to the following:

```
https://localhost:5001;http://localhost:5000
```

### Running from the Console

To run the app directly from the console, run the following command from this sample app's folder:

```powershell
$ dotnet run --project ./src/BasicCqrs.WebApp/BasicCqrs.WebApp.csproj --urls="https://localhost:5001;http://localhost:5000"
```

## Sample CLI Commands

```powershell
./scripts/cli-client-nosecurity.ps1 --help
```

```powershell
./scripts/cli-client-nosecurity.ps1 person --help
```

```powershell
./scripts/cli-client-nosecurity.ps1 person list
```

```powershell
./scripts/cli-client-nosecurity.ps1 person create --help
```

```powershell
$person = (./scripts/cli-client-nosecurity.ps1 person create --firstname "John" --lastname "Doe" | ConvertFrom-Json)
./scripts/cli-client-nosecurity.ps1 person update --id $person.id --lastname "Wick"
./scripts/cli-client-nosecurity.ps1 person delete --id $person.id
```
