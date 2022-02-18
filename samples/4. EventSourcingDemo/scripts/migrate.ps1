$rootDir = (get-item $PSScriptRoot).Parent.FullName

dotnet ef database update --startup-project $rootDir/src/EventSourcingDemo.WebApp/ --project $rootDir/src/EventSourcingDemo.SqlServer/ --context EventSourcingDemoDbContext
