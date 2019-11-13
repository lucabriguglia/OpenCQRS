# Basic

There is a single interface to be used, **IDispatcher** in Kledex namespace.
Note that all handlers are available as asynchronous as well as synchronous.

There are 3 kinds of messages:
- [Commands](https://github.com/lucabriguglia/Kledex/wiki/Commands) (single handler)
- [Events](https://github.com/lucabriguglia/Kledex/wiki/Events) (multiple handlers)
- [Queries](https://github.com/lucabriguglia/Kledex/wiki/Queries) (single handler)

Mapping between dispatcher methods and message handlers:

| Method | Handler |
| --- | --- |
| SendAsync | ICommandHandlerAsync |
| PublishAsync | IEventHandlerAsync |
| GetResultAsync | IQueryHandlerAsync |
