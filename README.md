# Brickweave
![Brickweave](https://github.com/agartee/brickweave/raw/master/assets/logo/brickweave200x200.png)

[![NuGet version](https://img.shields.io/nuget/v/LiteGuard.svg?style=flat)](https://www.nuget.org/packages?q=Brickweave)
[![Build status](https://ci.appveyor.com/api/projects/status/tjtip1xd6q11npio?svg=true)](https://ci.appveyor.com/project/agartee/brickweave)

## What is Brickweave?

Brickweave is a suite of .NET Standard 2.0 framework libraries to support developers on their Domain Driven Design journeys and provide clear, simple patterns to achieve DDD, CQRS, ES and domain messaging without getting bogged down with an overwhelming number of implementation decisions.

For full usage examples, see the included ***samples*** application.

## Brickweave.Domain

Contains a base ID model to help clarify model identity as well as improve code readability by adding type protection to identity values. In addition, it includes a custom JSON converter that can be used to flatten these IDs on serialization and un-flatten on deserialization.

### Sample ID model

```csharp
public class PersonId : Id<Guid> // implement Id value object with simple backing property type
{
    public PersonId(Guid value) : base(value) // ToString(), Equals() and GetHashCode() are overridden in base
    {
    }

    // factory method for generating a new Id
    public static PersonId NewId() 
    {
        return new PersonId(Guid.NewGuid());
    }
}
```

### Wiring-up the services (ASP.NET Core)

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc()
        .AddJsonOptions(options =>
            options.SerializerSettings.Converters.Add(new IdConverter())) // effects Id<T> implementations only

    ...
}
```

## Brickweave.Cqrs

Contains interface definitions for CQRS patterned commands and queries, as well as execution services to simplify DI requirements with the host application/API. It can work in tandem with the Brickweave.Domain library or without.

Commands and queries are separated to create clarity of intent within the application. This can be especially helpful for folks new to these patterns. Command and query handlers are services that define a single unit of work to process the request. This draws clear boundaries around the action and should be contained within an applications domain layer as opposed to directly in the UI or buried within support services and surrounded with condition checking. 

Command and Query handlers also come in two flavors: standard (e.g. `ICommandHandler` and `IQueryHandler`) and secured (e.g. `ISecuredCommandHandler` and `ISecuredQueryHandler`). Secured handlers work just like standard ones, but the secured versions are intended to perform authorization checks within the handler, and thus require a `ClaimsPrincipal` be passed along-side the command or query. No special action is required to differentiate between the two variants from the command/query executor level. The dispatcher service(s) will find the right one.

### Sample Command

```csharp
public class CreatePerson : ICommand<PersonInfo> // implement ICommand<T>, where T is the return type
{
    // recommended that commands and queries are read-only
    public CreatePerson(string firstName, string lastName, DateTime birthDate)
    {
        Id = PersonId.NewId();
        Name = new Name(firstName, lastName);
        BirthDate = birthDate;
    }

    public PersonId Id { get; }
    public Name Name { get; }
    public DateTime BirthDate { get; }
}
```

### Sample Query

```csharp
public class GetPerson : IQuery<PersonInfo> // implement IQuery<T>, where T is the return type
{
    public GetPerson(PersonId id) // utilizing Brickweave.Domain Id<T> implementation
    {
        Id = id;
    }

    public PersonId Id { get; }
}
```

### Sample Command Handler

```csharp
// define a command handler with it's handled command and return type
public class CreatePersonHandler : ICommandHandler<CreatePerson, PersonInfo>
{
    // inject any dependencies
    private readonly IPersonRepository _personRepository;

    public CreatePersonHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    // command handlers are async
    public async Task<PersonInfo> HandleAsync(CreatePerson command)
    {
        var person = new Person(command.Id, command.Name);

        await _personRepository.SavePersonAsync(person);
        
        return person.ToInfo(); // sample read-only return model transformed via custom extension method
    }
}
```

### Sample ASP.NET Controller

```csharp
public class PersonController : Controller
{
    // inject an IDispatcher
    private readonly IDispatcher _dispatcher;
    
    public PersonController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet, Route("/person/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        // dispatch a new query or command build from request args (example utilized Brickweave.Domain Id<T> model)
        var result = await _dispatcher.DispatchQueryAsync(new GetPerson(new PersonId(id)));

        return Ok(result);
    }

    [HttpPost, Route("/person/new")]
    public async Task<IActionResult> Create([FromBody] CreatePerson command)
    {
        // let ASP.NET deserialize the command object
        var result = await _dispatcher.DispatchCommandAsync(command);

        return Ok(result);
    }
}
```

### Wiring-up the services (ASP.NET Core)

The `IServiceCollection` extension method will perform assembly scans of the provided assemblies and register all implementations of command and query handlers as well as the other required services for handler routing to function.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // find the assemblies containing command/query handlers
    var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(a => a.FullName.StartsWith("MyApp"))
        .Where(a => a.FullName.Contains("Domain"))
        .ToArray();

    // utilize the extension method (from Brickweave.Cqrs.DependencyInjection)
    services.AddCqrs(domainAssemblies);

    ...
}
```

## Brickweave.EventStore

Contains base components to implement event-sourced aggregates and repositories. Snap-shotting/memento is not required and is not baked into these services and interfaces. Brickweave's event store was heavily influenced by the [NEventStore](https://github.com/NEventStore/NEventStore) project.

## Brickweave.EventStore.SqlServer

Contains base class to quickly and easily implement event-sourced repositories writing to a SQL Server database.

### Simple repository example (no snap-shots)

```csharp
public class SqlServerPersonRepository : SqlServerAggregateRepository<Person>, IPersonRepository
{
    private readonly SamplesDbContext _dbContext;

    public SqlServerPersonRepository(
        IDocumentSerializer serializer, // required Brickweave service
        IAggregateFactory aggregateFactory) // required Brickweave service
        : base(dbContext, serializer, aggregateFactory)
    {
        _dbContext = dbContext;
    }
    
    public async Task SavePersonAsync(Person person)
    {
        // add the aggregate's uncommitted events to the store
        AddUncommittedEvents(
            _dbContext.Events, // the DbSet<EventData> to use
            person, // the aggregate
            person.Id.Value); // the identifier for the event stream

        // todo: add snapshot(s) to DbContext if needed here

        await _dbContext.SaveChangesAsync(); // execute the transaction

        // perform aggregate cleanup
        person.ClearUncommittedEvents();
    }

    public async Task<Person> GetPersonAsync(PersonId id)
    {
        // todo: fetch from DbContext snapshot if needed here

        // fetch aggregate from event stream
        return await TryFindAsync(id.Value); // method in SqlServerAggregateRepository 
    }
}
```

*Note: If a snapshot/read-model is required for the application and needs to be immediately available for query, it is recommended to add the supporting tables to the same DbContext as the Event Store and perform those writes within the same transaction.*

### Wiring-up the services (ASP.NET Core)

The `IServiceCollection` extension method will perform assembly scans of the provided assemblies and register all implementations of the `IAggregateEvent` to simplify the JSON document being written to the database by removing assembly and namespace declarations in the document. This can significantly reduce the size of the database rows and simplify consumption of the stream by other application if accessed directly. 

The sample below (and in the sample project) also demonstrates how to configure Entity Framework to look for migration in another library, since migrations are not included in the `Brickweave.EventStore` library.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // find the assemblies containing event classes
    var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(a => a.FullName.StartsWith("Brickweave"))
        .Where(a => a.FullName.Contains("Domain"))
        .ToArray();

    // standard EF wire-up
    var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

    services.AddDbContext<SamplesDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("brickweave_samples"),
            sql => sql.MigrationsAssembly(GetMigrationAssemblyName())));
    
    // register the repository service
    services.AddScoped<IPersonRepository, SqlServerPersonRepository>();

    ...
}

private static string GetMigrationAssemblyName()
{
    return typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
}
```

## Brickweave.Cqrs.Cli

Contains services to support a command line application to easily execute commands and queries through a single API endpoint. As secured sample PowerShell CLI client is included at `/scripts/samples.ps1`. Users will be prompted for endpoint/authentication (I use Auth0) information the first time it is run and a `/scripts/samples.ps1.config` file will be generated (OAuth client secret us stored securely). In addition to configuring this client, a new API endpoint also needs to be created.

The CLI dispatcher works differently than the typical Brickweave CQRS dispatcher in that it must interpret the command text before dispatching the command or query. This is because it does not receive JSON data as it would from a typical web client. This also means ASP.NET must be configured to receive non-JSON data so it does not try to deserialize it into objects. An example plain-text-formatter can be found [here](samples/Brickweave.Samples.WebApp/Formatters/PlainTextInputFormatter.cs).

Help category definitions are defined in a separate JSON file and references in the `AddCli` services extension method.

### Sample Categories Help File

Create this file and reference its location when wiring-up the services.

```json
{
  "person": "Manage person data",
  "person phones": "Manage person phones",
  "person attributes": "Manage person attributes",
  "place": "Manage place data",
  "place things": "Manage place things"
}
```
Commands and Queries will by default be auto-discovered via text-case matching and by assuming the last "word" in your class definition is the "action" name. For example, the `CreatePerson` command translates to `person create` from the command line. This command/query text can be overridden via the `AddCli` services extension (see below). These overrides are handy when multiple commands should be organized into a single subject or category (e.g. `person`).

Command and Query parameters are passed with double-dashes (e.g. `--firstname "Adam"`) and the parameter names are not case-sensitive.

### Wiring-up the services (ASP.NET Core)

```csharp
private void ConfigureServices(IServiceCollection services)
{
    services.AddMvcCore(options =>
        {
            // see above for sample implementation of PlainTextInputFormatter
            options.InputFormatters.Add(new PlainTextInputFormatter());
        });

    services.AddCli(domainAssemblies)
        .AddDateParsingCulture(new CultureInfo("en-US"))
        .AddCategoryHelpFile("cli-categories.json") // contains "category" labels and description
        // optional overrides:
        .OverrideQueryName<ListPersons>("list", "person")
        .OverrideCommandName<AddMultiplePersonAttributes>("add-multiple", "person", "attributes")
}
```

### Sample CLI Controller

```csharp
public class CliController : Controller
{
    private readonly ICliDispatcher _cliDispatcher; // inject the CLI dispatcher

    public CliController(ICliDispatcher cliDispatcher)
    {
        _cliDispatcher = cliDispatcher;
    }

    [HttpPost, Route("/cli/run")]
    public async Task<IActionResult> Run([FromBody]string commandText) // plain text request body
    {
        // dispatch the command (as text)
        var result = await _cliDispatcher.DispatchAsync(commandText);

        // use the default formatter or create a custom one
        var value = result is HelpInfo info
            ? SimpleHelpFormatter.Format(info) : result;

        return Ok(value);
    }
}
```

### Add Command/Query Help Text

The CLI Help services utilize class summary descriptions. In order to utilize this feature, configure the project(s) containing commands and queries to generate an `XML documentation file`. It is also recommended to suppress warning [1591](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs1591) in Visual Studio. 

![XML Doc](https://github.com/agartee/brickweave/raw/master/assets/doc_xml.png)

### Sample Class Documentation (Command and Queries, not Handlers)

```csharp
public class CreatePerson : ICommand<PersonInfo>
{
    /// <summary>
    /// Create a new person.
    /// </summary>
    /// <param name="firstName">Person's first name</param>
    /// <param name="lastName">Person's last name</param>
    /// <param name="birthDate">Person's birth date</param>
    public CreatePerson(string firstName, string lastName, DateTime birthDate)
    {
        Id = PersonId.NewId();
        Name = new Name(firstName, lastName);
        BirthDate = birthDate;
    }

    public PersonId Id { get; }
    public Name Name { get; }
    public DateTime BirthDate { get; }
}
```

### Sample Command Line Output

![Sample Output](https://github.com/agartee/brickweave/raw/master/assets/cli_outputs.png)

### Advanced Command Text Samples

The `CliDispatcher` supports the following parameter value definitions.

#### Single Value
A single parameter value sush as `int`, `Guid`, `DateTime`, etc. These values can be wrapped in double-quotes if they contain spaces.

```powershell
person create --firstName "Adam"
```

#### List Values
Multiple values can be added as a collection by separating values with a space from the command line and defining the command/query parameter as an `IEnumerable<T>`, `IList<T>` or `List<T>`.

```powershell
person phones add --personid 00112233-4455-6677-8899-aabbccddeeff --phones "555-1111" "555-2222"
```

#### Dictionary Value
Multiple values in the form of key/value pairs are defined by wrapping the value in square braces with a preceding equals sign. Dictionary key values can also be wrapped in double-quotes to allow for spaces.

```powershell
person phones add --personid 00112233-4455-6677-8899-aabbccddeeff --attributes Height[=pretty tall] "Favorite Color"[=Orange]
```

#### Primitive-Wrapped Object Values
If a command/query contains a value-object that contains a single primitive-type or Guid constructor, it can be interpreted. This is useful when utilizing `Brickweave.Domain` `Id<T>` objects.

```powershell
person get --personid 00112233-4455-6677-8899-aabbccddeeff
```

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

public class GetPerson : IQuery<PersonInfo>
{
    public GetPerson(PersonId id) // Id object param
    {
        Id = id;
    }

    public PersonId Id { get; }
}
```

## Running the Project from Source

This project is configured to use user secrets for integration tests. Below are the settings needed to run the tests. The sample application is configured to use a simple Identity Server 4 server (included in samples).

```json
{
  "connectionStrings": {
    "brickweave_tests": "server=[server name];database=[database name];integrated security=true;",
    "brickweave_samples": "server=[server name];database=[database name];integrated security=true;",
    "serviceBus": "Endpoint=sb://[service bus namespace].servicebus.windows.net/;SharedAccessKeyName=[key name];SharedAccessKey=[secret]"
  },

  "serviceBusTopic": "[service bus topic name]",
  "serviceBusSubscription": "[service bus subscription name]",

  "authentication": {
    "authority": "https://[your namespace].auth0.com/",
    "audience": "[your Auth0 API name]",
    "client_id": "[your Auth0 client id]",
    "client_secret": "[your Auth0 client secret]",
    "grant_type": "client_credentials"
  } 
}
```
