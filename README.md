# Weapsy.CQRS

[![Build status](https://ci.appveyor.com/api/projects/status/p5p80y0fa6e9wbaa?svg=true)](https://ci.appveyor.com/project/lucabriguglia/weapsy.cqrs)

CQRS and Event Sourcing library for .NET Core.

## Installing Weapsy.CQRS

Nuget Packages

[![Nuget Package](https://img.shields.io/badge/Weapsy.Cqrs-4.0.1-blue.svg)](https://www.nuget.org/packages/Weapsy.Cqrs)

[![Nuget Package](https://img.shields.io/badge/Weapsy.Cqrs.Store.CosmosDB.MongoDB-4.0.1-blue.svg)](https://www.nuget.org/packages/Weapsy.Cqrs.Store.CosmosDB.MongoDB)

[![Nuget Package](https://img.shields.io/badge/Weapsy.Cqrs.Store.CosmosDB.Sql-4.0.1-blue.svg)](https://www.nuget.org/packages/Weapsy.Cqrs.Store.CosmosDB.Sql)

[![Nuget Package](https://img.shields.io/badge/Weapsy.Cqrs.Store.EF.MySql-4.0.1-blue.svg)](https://www.nuget.org/packages/Weapsy.Cqrs.Store.EF.MySql)

[![Nuget Package](https://img.shields.io/badge/Weapsy.Cqrs.Store.EF.PostgreSql-4.0.1-blue.svg)](https://www.nuget.org/packages/Weapsy.Cqrs.Store.EF.PostgreSql)

[![Nuget Package](https://img.shields.io/badge/Weapsy.Cqrs.Store.EF.Sqlite-4.0.1-blue.svg)](https://www.nuget.org/packages/Weapsy.Cqrs.Store.EF.Sqlite)

[![Nuget Package](https://img.shields.io/badge/Weapsy.Cqrs.Store.EF.SqlServer-4.0.1-blue.svg)](https://www.nuget.org/packages/Weapsy.Cqrs.Store.EF.SqlServer)

Via Package Manager

    Install-Package Weapsy.Cqrs
    
Or via .NET CLI

    dotnet add package Weapsy.Cqrs
    
Or via Paket CLI

    paket add Weapsy.Cqrs

For Event Sourcing, an event store data provider needs to be installed.
There are few already available but more are coming up.
Install one between CosmosDB Sql (DocumnentDB), CosmosDB MongoDB, SqlServer, MySql, PostgreSql and Sqlite.
The following example is for the MongoDB package.

Via Package Manager

    Install-Package Weapsy.Cqrs.Store.CosmosDB.MongoDB
    
Or via .NET CLI

    dotnet add package Weapsy.Cqrs.Store.CosmosDB.MongoDB


Or via Paket CLI

    paket add Weapsy.Cqrs.Store.CosmosDB.MongoDB

## Using Weapsy.CQRS

Working examples for different database providers are available in the examples folder of the repository https://github.com/Weapsy/Weapsy.CQRS/tree/master/examples

### Register services

In ConfigureServices method of Startup.cs:

```C#
services.AddWeapsyCqrs(typeof(CreateProduct), typeof(GetProduct));
```

CreateProduct is an sample command and GetProduct is a sample query.
In this scenario, commands and queries are in two different assemblies.
Both assemblies need to be registered.
In order to use the event sourcing functionalities, an event store provider needs to be added as well.
Weapsy.CQRS currently supports the following data providers:
- CosmosDB SQL (DocumentDB)
- CosmosDB MongoDB
- SqlServer
- MySql
- PostgreSql
- Sqlite

Please install the nuget package of your choice and register the event store:

```C#
services.AddWeapsyCqrsSqlServerProvider(Configuration);
```

In order to use CosmosDB you need to install the free emulator (https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator) and add some settings to the appsettings.json.

For CosmosDB SQL (DocumentDB):

```JSON
{
  "DomainDbConfiguration": {
    "ServerEndpoint": "https://localhost:8081",
    "AuthKey": "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
    "DatabaseId": "DomainStore",
    "AggregateCollectionId": "Aggregates",
    "CommandCollectionId": "Commands",
    "EventCollectionId": "Events"
  }
}
```

For CosmosDB MongoDB:

```JSON
{
  "DomainDbConfiguration": {
    "ConnectionString": "mongodb://localhost:C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==@localhost:10255/admin?ssl=true",
    "DatabaseName": "DomainStore",
    "AggregateCollectionName": "Aggregates",
    "CommandCollectionName": "Commands",
    "EventCollectionName": "Events"
  }
}
```

And add the following check in the Configure method (for CosmosDB SQL (DocumentDB) only):

```C#
public void Configure(IApplicationBuilder app, IOptions<CosmosDBSettings> settings)
{
    app.EnsureDomainDbCreated(settings);
}
```

For all the others based on Entity Framework Core add just the connection string to appsettings.json:

```JSON
{
  "DomainDbConfiguration": {
    "ConnectionString": "Server=(localdb)\\mssqllocaldb;Database=DomainDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

And the following line can be added to the Configure method to ensure that the database in installed:

```C#
public void Configure(IApplicationBuilder app, DomainDbContext domainDbContext)
{
    domainDbContext.Database.Migrate();
}
```

### Basics

There is a single interface to be used, IDispatcher in Weapsy.Cqrs namespace.
Note that all handlers are available as asynchronous and as well as synchronous, but for these examples I'm using the asynchronous versions only.

There are 3 kinds of messages:
- Command, single handler
- Event, multiple handlers
- Query/Result, single handler that returns a result

#### Command (simple usage)

First, create a message:

```C#
public class DoSomething : ICommand
{
}
```

Next, create the handler:

```C#
public class DoSomethingHandlerAsync : ICommandHandlerAsync<DoSomething>
{
    public Task HandleAsync(DoSomething command)
    {
        await _myService.MyMethodAsync();
    }
}
```

And finally, send the command using the dispatcher:

```C#
var command = new DoSomething();
await _dispatcher.SendAsync(command)
```

#### Command (with events)

Using the SendAndPublishAsync method, the dispatcher will automatically publish the events returned by the handler.

First, create a command and an event:

```C#
public class DoSomething : ICommand
{
}

public class SomethingHappened : IEvent
{
}
```

Next, create the handler:

```C#
public class DoSomethingHandlerAsync : ICommandHandlerWithEventsAsync<DoSomething>
{
    public Task<IEnumerable<IEvent>> HandleAsync(DoSomething command)
    {
        await _myService.MyMethodAsync();
        return new List<IEvent>{new SomethingHappened()};
    }
}
```

And finally, send the command and publish the events using the dispatcher:

```C#
var command = new DoSomething();
await _dispatcher.SendAndPublishAsync(command)
```

#### Event

First, create a message:

```C#
public class SomethingHappened : IEvent
{
}
```

Next, create one or more handlers:

```C#
public class SomethingHappenedHandlerAsyncOne : IEventHandlerAsync<SomethingHappened>
{
    public Task HandleAsync(SomethingHappened @event)
    {
        await _myService.MyMethodAsync();
    }
}

public class SomethingHappenedHandlerAsyncTwo : IEventHandlerAsync<SomethingHappened>
{
    public Task HandleAsync(SomethingHappened @event)
    {
        await _myService.MyMethodAsync();
    }
}
```

And finally, publish the event using the mediator:

```C#
var @event = new SomethingHappened();
await _dispatcher.PublishAsync(@event)
```

#### Query/Result

First, create a model and a message:

```C#
public class Something
{
    public int Id { get; set; }
}

public class GetSomething : ICommand
{
    public int Id { get; set; }
}
```

Next, create the handler:

```C#
public class GetSomethingQueryHandlerAsync : IQueryHandlerAsync<GetSomething, Something>
{
    public async Task<Something> RetrieveAsync(GetSomething query)
    {
        return await _db.Somethings.FirstOrDefaultAsync(x => x.Id == query.Id);
    }
}
```

And finally, get the result using the dispatcher:

```C#
var query = new GetSomething{ Id = 123 };
var something = await _dispatcher.GetResultAsync<GetSomething, Something>(query);
```

### Event Sourcing

Using the SendAndPublishAsync<IDomainCommand, IAggregateRoot> method, the dispatcher will automatically publish the events of the aggregate returned by the handler and save those events to the event store.
A working example can be found at https://github.com/Weapsy/Weapsy.CQRS/tree/master/examples

First, create a command and an event:

```C#
public class CreateProduct : DomainCommand
{
    public string Title { get; set; }
}

public class ProductCreated : DomainEvent
{
    public string Title { get; set; }
}
```

Next, create a domain object that inherits from AggregateRoot.
This is how a Procuct class might look like:

```C#
public class Product : AggregateRoot
{
    public string Title { get; private set; }

    public Product()
    {            
    }

    public Product(Guid id, string title) : base(id)
    {
        if (string.IsNullOrEmpty(title))
            throw new ApplicationException("Product title is required.");

        AddEvent(new ProductCreated
        {
            AggregateId = Id,
            Title = title
        });
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
            throw new ApplicationException("Product title is required.");

        AddEvent(new ProductTitleUpdated
        {
            AggregateId = Id,
            Title = title
        });
    }

    public void Apply(ProductCreated @event)
    {
        Id = @event.AggregateId;
        Title = @event.Title;
    }

    public void Apply(ProductTitleUpdated @event)
    {
        Title = @event.Title;
    }
}
```

Note that the empty constructor is required in order to create a new object.
After every command has been executed, an event is added to the pending event list calling the AddEvent method.
The Apply methods are called automatically when new events are added and are also used to load the object from the history when GetById method of the Repository is called.
Create the first handler:

```C#
public class CreateProductHandlerAsync : ICommandHandlerWithAggregateAsync<CreateProduct>
{
    public async Task<IAggregateRoot> HandleAsync(CreateProduct command)
    {
        await Task.CompletedTask;

        var product = new Product(command.AggregateId, command.Title);

        return product;
    }
}
```

Send the command using the dispatcher:

```C#
var command = new CreateProduct
{
    AggregateId = Guid.NewGuid(),
    Title = "My brand new product"
};
await _dispatcher.SendAndPublishAsync<CreateProduct, Product>(command)
```

At this stage, we might want to create our read model.
It can be achieved by creating an event handler:

```C#
public class ProductCreatedHandlerAsync : IEventHandlerAsync<ProductCreated>
{
    public async Task HandleAsync(ProductCreated @event)
    {
        await Task.CompletedTask;

        var model = new ProductViewModel
        {
            Id = @event.AggregateId,
            Title = @event.Title
        };

        FakeReadDatabase.Products.Add(model);
    }
}
```

At this point, the aggregate and the first event have been saved to the event store and the product can be retrieved from the event store using the repository.

New commands, events and handlers can now be added:

```C#
public class UpdateProductTitle : DomainCommand
- {
    public string Title { get; set; }
}

public class ProductTitleUpdated : DomainEvent
{
    public string Title { get; set; }
}

public class UpdateProductTitleHandlerAsync : ICommandHandlerWithAggregate<UpdateProductTitle>
{
    private readonly IRepository<Product> _repository;

    public UpdateProductTitleHandlerAsync(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public async Task<IAggregateRoot> HandleAsync(UpdateProductTitle command)
    {
        var product = await _repository.GetByIdAsync(command.AggregateId);

        if (product == null)
            throw new ApplicationException("Product not found.");

        product.UpdateTitle(command.Title);

        return product;
    }
}

public class ProductTitleUpdatedHandlerAsync : IEventHandlerAsync<ProductTitleUpdated>
{
    public async Task HandleAsync(ProductTitleUpdated @event)
    {
        await Task.CompletedTask;

        var model = FakeReadDatabase.Products.Find(x => x.Id == @event.AggregateId);
        model.Title = @event.Title;
    }
}
```

As per prevoius example, the dispatcher can be used to update the product.

```C#
await dispatcher.SendAndPublishAsync<UpdateProductTitle, Product>(new UpdateProductTitle
{
    AggregateId = productId,
    Title = "Updated product title"
});
```

A new event is saved and the read model is updated using the event handler.
Next time the aggregate is loaded from the repository, two events will be applied in order to recreate the current state.

Note the two optional properties can be saved for the domain events:
- UserId (Guid)
- Source (string)
