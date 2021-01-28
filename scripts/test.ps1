Param(
  [Parameter(Mandatory=$false, HelpMessage="Configuration name (e.g. Release, Debug)")]
  [string]$configuration = "Debug"
)
$status = 0
$rootDir = (get-item $PSScriptRoot).Parent.FullName
$testProjects = Get-ChildItem -Path $rootDir\tests -Filter *.csproj -Recurse -File| ForEach-Object { $_.FullName }

foreach ($testProject in $testProjects)
{
    dotnet test $testProject --no-build -c $configuration

    if ($status -eq 0) {
        $status = $LASTEXITCODE
    }
}

exit $status