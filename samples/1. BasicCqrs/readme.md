# Brickweave Basic CQRS Demo

## Running the App

This demo requires no additional configuration. It can simply be run as-is.

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
