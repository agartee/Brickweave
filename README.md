[![NuGet version](https://img.shields.io/nuget/v/LiteGuard.svg?style=flat)](https://www.nuget.org/packages?q=Brickweave)
[![Build status](https://ci.appveyor.com/api/projects/status/gk6r7gupecbc23rn?svg=true)](https://ci.appveyor.com/project/agartee/brickweave)

# What is Brickweave?

Brickweave is a suite of .NET Standard 2.0 framework libraries to support developers on their Domain Driven Design journeys and provide clear, simple patterns to achieve DDD, CQRS, ES and domain messaging without getting bogged down with an overwhelming number of implementation decisions.

For full usage examples, see the included `samples` application.

# Brickweave.Domain

Contains a base ID model to help clarify model identity as well as improve code readability by adding type protection to identity values. In addition, it includes a custom JSON converter to flatten these IDs on serialization if client/consuming application require a format change.

## Sample ID model

```csharp
public class PersonId : Id<Guid>
{
    public PersonId(Guid value) : base(value)
    {
    }

    public static PersonId NewId()
    {
        return new PersonId(Guid.NewGuid());
    }
}
```

## Wiring-up the services (ASP.NET Core)

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc()
        .AddJsonOptions(options =>
            options.SerializerSettings.Converters.Add(new IdConverter()))

    ...
}
```

# Brickweave.Cqrs

Contains interface definitions for CQRS patterned commands and queries, as well as execution services to simplify DI requirements with the host application/API. It can work in tandem with the Brickweave.Domain library or without.

Commands and queries are separated to create clarity of intent within the application. This can be especially helpful for folks new to these patterns. Command and query handlers are services that define a single unit of work to process the request. This draws clear boundries around the action and should be contained within an applications domain layer as opposed to directly in the UI or buried within support services and surrounded with condition checking. 

Command and Query processors also come in two flavors: standard (e.g. `ICommandHandler` and `IQueryHandler`) and secured (e.g. `ISecuredCommandHandler` and `ISecuredQueryHandler`). Secured handlers work just like standard ones, but the secured versions are indended to perform authorization checks within the handler, and thus require a `ClaimsPrincipal` be passed along-side the command or query. No special action is required to differentiate between the two variants from the command/query executor level. The executor will find the right one.

Have a look at the sample project to get a feel for DI container wire-up.

## Simple ASP.NET Controller example

```csharp
public class PersonController : Controller
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly IQueryExecutor _queryExecutor;

    public PersonController(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
    {
        _commandExecutor = commandExecutor;
        _queryExecutor = queryExecutor;
    }

    [HttpGet, Route("/person/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _queryExecutor.ExecuteAsync(new GetPerson(new PersonId(id)));

        return Ok(result);
    }

    [HttpPost, Route("/person/new")]
    public async Task<IActionResult> Create([FromBody] CreatePersonRequest request)
    {
        var result = await _commandExecutor.ExecuteAsync(new CreatePerson(
            PersonId.NewId(), new Name(request.FirstName, request.LastName)));

        return Ok(result);
    }
}
```

## Wiring-up the services (ASP.NET Core)

The `IServiceCollection` extension method will perform assembly scans of the provided assemblies and register all implementations of command and query handlers as well as the other required services for handler routing to function.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(a => a.FullName.StartsWith("Brickweave"))
        .Where(a => a.FullName.Contains("Domain"))
        .ToArray();

    services.AddCqrs(domainAssemblies);

    ...
}
```

# Brickweave.EventStore

Includes base components to implement event-sourced aggregates and repositories. Snap-shotting/memento is not required and is not baked into these services and interfaces.

# Brickweave.EventStore.SqlServer

Contains base class to quickly and easily implement event-sourced repositories writing to a SQL Server database.

## Simple repository example (no snap-shots)

```csharp
public class SqlServerPersonRepository : SqlServerAggregateRepository<Person>, IPersonRepository
{
    public SqlServerPersonRepository(EventStoreContext dbContext, IDocumentSerializer serializer, IAggregateFactory aggregateFactory) 
        : base(dbContext, serializer, aggregateFactory)
    {
    }
    
    public async Task SavePersonAsync(Person person)
    {
        await SaveUncommittedEventsAsync(person, person.Id.Value);
    }

    public async Task<Person> GetPersonAsync(PersonId id)
    {
        return await TryFindAsync(id.Value);
    }
}
```

## Wiring-up the services (ASP.NET Core)

The `IServiceCollection` extension method will perform assembly scans of the provided assemblies and register all implementations of the `IAggregateEvent` to simplify the JSON document being written to the database by removing assembly and namespace declarations in the document. This can significantly reduce the size of the database rows and simplify consumption of the stream by other application if accessed directly. 

The sample below (and in the sample project) also demonstrates how to configure Entity Framework to look for migration in another library, since migrations are not included in the `Brickweave.EventStore` library.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(a => a.FullName.StartsWith("Brickweave"))
        .Where(a => a.FullName.Contains("Domain"))
        .ToArray();

    services.AddEventStore(domainAssemblies)
        .AddDbContext(options => options.UseSqlServer(Configuration.GetConnectionString("brickweave_samples"),
            sql => sql.MigrationsAssembly(migrationsAssembly)));
    
    services.AddScoped<IPersonRepository, SqlServerPersonRepository>();

    ...
}
```

# Brickweave.Messaging

Description coming soon!

# Brickweave.Messaging.MessageBus

Description coming soon!

# Brickweave.Cqrs.Cli

Description coming soon!