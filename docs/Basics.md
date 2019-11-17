# Basics

The **IDispatcher** interface contains all the methods needed to send commands, publish events and get results from queries.

The **IRepository<T>** interface can be used to load an aggregate from history by replaying all the associated events.
  
Note that all methods and handlers are available as asynchronous as well as synchronous.

There are 3 kinds of messages:
- [Commands](https://github.com/lucabriguglia/Kledex/wiki/Commands) (single handler)
- [Events](https://github.com/lucabriguglia/Kledex/wiki/Events) (multiple handlers)
- [Queries](https://github.com/lucabriguglia/Kledex/wiki/Queries) (single handler)

Mapping between dispatcher methods and handlers:

| Method | Handler |
| --- | --- |
| SendAsync | ICommandHandlerAsync |
| PublishAsync | IEventHandlerAsync |
| GetResultAsync | IQueryHandlerAsync |

It's also possible to use the following interfaces directly without going through the framework flows:
- ICacheManager
- IValidationService
