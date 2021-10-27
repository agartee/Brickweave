$rootDir = (get-item $PSScriptRoot).Parent.FullName
dotnet ef database update --startup-project $rootDir/src/AdvancedCqrs.WebApp/ --project $rootDir/src/AdvancedCqrs.SqlServer/
