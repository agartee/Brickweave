$rootDir = (get-item $PSScriptRoot).Parent.FullName
dotnet ef database update --startup-project $rootDir/src/AdvancedCqrs.WebApp/ --project $rootDir/src/AdvancedCqrs.SqlServer/
dotnet ef database update --startup-project $rootDir/src/AdvancedCqrs.CommandQueue.WebApp/ --project $rootDir/src/AdvancedCqrs.CommandQueue.SqlServer/