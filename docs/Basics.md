# Basics

The **IDispatcher** interface contains all the methods needed to send commands and publish events.

The **IRepository<T>** interface can be used to load an aggregate from history by replaying all the associated events.
  
Note that all methods and handlers are available as asynchronous as well as synchronous.

There are 2 kinds of messages:
- [Commands](Commands) (single handler)
- [Events](Events) (multiple handlers)

It's also possible to use the following interfaces directly without going through the framework flows:
- **ICacheManager**
- **IValidationService**

## Handlers

The following is the mapping between dispatcher methods and handlers:

| Method | Parameter | Handler |
| --- | --- | --- |
| SendAsync | ICommand | ICommandHandlerAsync or Custom |
| SendAsync | ICommandSequence | ISequenceCommandHandlerAsync |
| PublishAsync | IEvent | IEventHandlerAsync |

## Main Flow

![Send Command Flow](assets/img/SendCommandFlow.svg)
