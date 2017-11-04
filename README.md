# Weapsy.Mediator

[![Build status](https://ci.appveyor.com/api/projects/status/p5p80y0fa6e9wbaa?svg=true)](https://ci.appveyor.com/project/lucabriguglia/weapsy-mediator)
[![Nuget Package](https://img.shields.io/badge/nuget-1.0.0-brightgreen.svg)](https://www.nuget.org/packages/Weapsy.Mediator)

Simple mediator for .NET Core

## Installing Weapsy.Mediator

[Nuget Package](https://www.nuget.org/packages/Weapsy.Mediator/)

Via Package Manager

    Install-Package Weapsy.Mediator
    
Or via .NET CLI

    dotnet add package Weapsy.Mediator
    
Or via Paket CLI

    paket add Weapsy.Mediator

## Using Weapsy.Mediator

### Register services

In this example I'm using [Scrutor](https://github.com/khellang/Scrutor).

```C#
services.Scan(s => s
    .FromAssembliesOf(typeof(IMediator), typeof(DoSomething))
    .AddClasses()
    .AsImplementedInterfaces());
```
DoSomething is an sample command and the assumption is that all the handlers are in the same assembly.

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




























