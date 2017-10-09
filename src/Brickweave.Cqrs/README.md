# Brickweave.Cqrs

Lightweight package to support the CQRS pattern stand-alone or with other Brickweave products.

## Command Usage

Define a command object with with parameters.

### Example: 

``` csharp
public class SampleCommand : ICommand
{
    public SampleCommand(string someValue)
    {
        SomeValue = someValue;
    }

    public string SomeValue { get; }
}
```

Define a command handler that takes the command as a generic type arg and add your application logic.

### Example: 

``` csharp
public class SampleCommandHandler: ICommandHandler<SampleCommand>
{
    public async Task HandleAsync(SampleCommand command)
    {
        // your code here
    }
}

```

Sometimes it is useful to return a result object when a command is successfully processed (e.g. returning a read-only model of the aggregate that was created/modified, etc.).

### Example: 

``` csharp
public class CreatePersonHandler: ICommandHandler<CreatePerson, PersonInfo>
{
    public async Task HandleAsync(CreatePerson command)
    {
        var person = new Person
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe"
        }

        // todo: save person to the data store

        return new PersonInfo(person.Id, person.FirstName, person.LastName);
    }
}

```

## Query Usage

Define a query object with with parameters.

### Example: 

``` csharp
public class SampleQuery : IQuery<SampleResult>
{
    public SampleQuery(string someValue)
    {
        SomeValue = someValue;
    }

    public string SomeValue { get; }
}
```

Similarly to the command handler, define a query handler that takes the query as a generic type arg and add your application logic. Unlike commands and command handlers, queries require a definied result type.

### Example: 

``` csharp
public class SampleQueryHandler: IQueryHandler<SampleQuery, SampleResult>
{
    public async Task HandleAsync(SampleQuery query)
    {
        // fetch from the data store

        return new SampleResult(data.foo, data.bar);
    }
}

```