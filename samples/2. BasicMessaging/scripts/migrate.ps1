$rootDir = (get-item $PSScriptRoot).Parent.FullName

dotnet ef database update --startup-project $rootDir/src/BasicMessaging.WebApp/ --project $rootDir/src/BasicMessaging.SqlServer/ --context BasicMessagingDbContext
