$rootDir = (get-item $PSScriptRoot).Parent.FullName
dotnet ef database update --startup-project $rootDir/src/AdvancedCqrs.WebApp/ --project $rootDir/src/AdvancedCqrs.SqlServer/ --context AdvancedCqrsDbContext
dotnet ef database update --startup-project $rootDir/src/AdvancedCqrs.WebApp/ --project $rootDir/src/AdvancedCqrs.CommandQueue.SqlServer/ --context CommandQueueDbContext
