# Brickweave.Cqrs

Brickweave CQRS is a lightweight framework library to support the CQRS pattern within a single application. Consuming applications would define commands, queries and their handlers to wrap business logic into a nice little unit of work. This not only helps teams distribute work more easily, but also helps them predict where business logic should be placed without a lot of debate.

# Getting Started

The following examples are also located in the `Brickweave.Samples` projects in this repository. These examples are simple, but often application commands and queries will require manipulation of multiple domain models, publish domain messages, or perform some aggregation of data to produce results. That being said, these samples should be sufficient to get started.

## Step 1: Wire-Up

To have your application start recognizing newly defined commands and queries, use the `IServiceCollection` extension methods to wire-up the Brickweave services. Since these services rely on dependency injection via .NET Core's `IServiceCollection`, you will want to wire-up your own domain services as well. Not that assemblies containing your command and queries can be passed to these extension methods and an assembly scan for `IQueryHandler` and `ICommandHandler` services will be executed.

This example is for ASP.NET Core, but for console or other applications a new `ServiceCollection` instance can be created and utilized as well.

### Example Wire-Up (ASP.NET Core):

``` csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc();

    // find libraries that will contain my command and query handlers
    var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(a => a.FullName.StartsWith("MyApplication")) // your namespace prefix
        .Where(a => a.FullName.Contains("Domain")) // any additional criteria to isolate your domain libraries
        .ToArray();

    services.AddCqrs(domainAssemblies);

    ...
}

```

## Step 2: Define Command and Query

Now that your application is setup, you can add commands and queries to your domain libraries and their handlers will be automatically picked up without any additional DI container registrations (except for your additional domain services).

Lets start by defining a command and a query.

### Example Command:

``` csharp
public class CreatePerson : ICommand
{
    public CreatePerson(Guid personId, string firstName, string lastName)
    {
        PersonId = personId;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid PersonId { get; }
    public string FirstName { get; }
    public string LastName { get; }
}
```

### Example Query:

``` csharp
public class GetPerson : IQuery<PersonInfo>
{
    public GetPerson(Guid personId)
    {
        PersonId = personId;
    }

    public Guid PersonId { get; }
}
```

***Note: Commands can be configured to return a result just like Queries***

## Step 3: Define Command and Query Handlers

Now that we have a command and a query, we can define services that will handle them. These services will be located via the `CommandExecutor` and `QueryExecutor` services, which will be referenced later by your ASP.NET Core controller class.

### Example Command Handler:

``` csharp
public class CreatePersonHandler : ICommandHandler<CreatePerson>
{
    private readonly IPersonRepository _personRepository;

    public CreatePersonHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<PersonInfo> HandleAsync(CreatePerson command)
    {
        var person = new Person
        {
            Id = command.PersonId,
            FirstName = command.FirstName,
            LastName = command.LastName
        };

        await _personRepository.SaveAsync(person);
    }
}
```

### Example Query Handler:

``` csharp
public class GetPersonHandler : IQueryHandler<GetPerson, PersonInfo>
{
    private readonly IPersonRepository _personRepository;

    public GetPersonHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<PersonInfo> HandleAsync(GetPerson query)
    {
        var person = await _personRepository.GetPersonAsync(query.Id);
        return person.ToInfo();
    }
}
```

## Step 4: Controller Wire-up

The last step of this example is to expose API endpoints for the newly defined command and query. The only services an application should need to reference are the `ICommandExecutor` and `IQueryExecutor`. From there, data from the HTTP request should be mapped into a new command/query model, and the command/query is submitted to the Executor. That's it!

``` csharp
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
        var result = await _queryExecutor.ExecuteAsync(new GetPerson(id));

        return Ok(result);
    }

    [HttpPost, Route("/person/new")]
    public async Task<IActionResult> Create([FromBody] CreatePersonRequest request)
    {
        await _commandExecutor.ExecuteAsync(new CreatePerson(
            Guid.NewGuid(), request.FirstName, request.LastName));

        return Created();
    }
}
```

# Final Thoughts

That's it! Clean and simple. Stay tuned to updates in the Wiki to see more advanced usages of this library, including security integration (coming soon).