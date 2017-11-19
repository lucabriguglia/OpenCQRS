# Weapsy.Mediator

[![Build status](https://ci.appveyor.com/api/projects/status/p5p80y0fa6e9wbaa?svg=true)](https://ci.appveyor.com/project/lucabriguglia/weapsy-mediator)

Mediator for .NET Core

## Installing Weapsy.Mediator

Nuget Packages

[![Nuget Package](https://img.shields.io/badge/Weapsy.Mediator-1.1.0-brightgreen.svg)](https://www.nuget.org/packages/Weapsy.Mediator)
[![Nuget Package](https://img.shields.io/badge/Weapsy.Mediator.EventStore.EF-1.1.0-brightgreen.svg)](https://www.nuget.org/packages/Weapsy.Mediator.EventStore.EF)

Via Package Manager

    Install-Package Weapsy.Mediator
    
Or via .NET CLI

    dotnet add package Weapsy.Mediator
    
Or via Paket CLI

    paket add Weapsy.Mediator

For Event Sourcing, an event store implementation needs to be installed.
The only available at the minute is the entity framework event store.

Via Package Manager

    Install-Package Weapsy.Mediator.EventStore.EF
    
Or via .NET CLI

    dotnet add package Weapsy.Mediator.EventStore.EF
    
Or via Paket CLI

    paket add Weapsy.Mediator.EventStore.EF

## Using Weapsy.Mediator

A fully working example, including CQRS and Event Sourcing, is available in the examples folder of the repository https://github.com/Weapsy/Weapsy.Mediator/tree/master/examples

### Register services

In ConfigureServices of Startup.cs:

```C#
services.AddWeapsyMediator(typeof(CreateProduct), typeof(GetProduct));
```

CreateProduct is an sample command and GetProduct is a sample query.
In this scenario, commands and queries are in two different assemblies.
Both assemblies need to be registered.
In order to use the event sourcing functionalities, an event store needs to be registered.

```C#
services.AddWeapsyEFEventStore();
```

At the moment, the only available is the Entity Framework event store but more will be available soon like Blob Storage or Xml.
The EFEventStore uses a db context and it needs to be configured.
Any of the data providers currently supported in entity framework core can be used.
In this case I'm using SqlServer:

```C#
services.AddDbContext<MediatorDbContext>(options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EventStore;Trusted_Connection=True;MultipleActiveResultSets=true"));
```

If entity framework is used, databse can be installed adding this line in the Configure method of Startup.cs:

```C#
mediatorDbContext.Database.Migrate();
```

mediatorDbContext is passed as a parameter:

```C#
public void Configure(IApplicationBuilder app, IHostingEnvironment env, MediatorDbContext mediatorDbContext)
```

### Basics

Note that all handlers are available as asynchronous and as well as synchronous, but for these examples I'm using the asynchronous ones only.

There are 3 kinds of messages:
- Command, single handler
- Event, multiple handlers
- Query/Result, single handler that returns a result

#### Command (Simple usage)

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

And finally, send the command using the mediator:

```C#
var command = new DoSomething();
await _mediator.SendAsync(command)
```

#### Command (With events)

Using the SendAndPublishAsync method, the mediator will automatically publish the events returned by the handler.

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

And finally, send the command and publish the events using the mediator:

```C#
var command = new DoSomething();
await _mediator.SendAndPublishAsync(command)
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
await _mediator.PublishAsync(@event)
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

And finally, get the result using the mediator:

```C#
var query = new GetSomething{ Id = 123 };
var something = await _mediator.GetResultAsync<GetSomething, Something>(query);
```




























