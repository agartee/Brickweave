param(
    [Parameter(Mandatory, HelpMessage="Migration Name. Defaults to latest migration.")]
    [AllowEmptyString()]
    [string]$migrationName
)

$start = get-date -Format "dddd MM/dd/yyyy HH:mm"
Write-Host "Starting migration at $start"

$rootDir = (get-item $PSScriptRoot).Parent.FullName

dotnet ef database update $migrationName --startup-project $rootDir/src/BasicMessaging.WebApp/ --project $rootDir/src/BasicMessaging.SqlServer/

$end = get-date -Format "dddd MM/dd/yyyy HH:mm"
Write-Host "Migration completed at $end"
