# Kledex

Kledex is a .NET Core framework that can be used to create a simple and clean design by enforcing single responsibility and separation of concerns.
Its advanced features are ideal for Domain Driven Design (DDD), Command Query Responsibilty Segragation (CQRS) and Event Sourcing.
With Kledex you can automatically dispatch events to a message bus (Service Bus or RabbitMQ), validate your commands before they are sent to the command handler and automatically cache the result of your queries (Memory or Redis).

- [Installation](Installation)
- [Configuration](Configuration)
- [Basics](Basics)
   - [Commands](Commands)
   - [Events](Events)
   - [Queries](Queries)
- [Domain](Domain)
   - [With Event Sourcing](With-Event-Sourcing)
   - [Without Event Sourcing](Without-Event-Sourcing)
- [Message Bus](Message-Bus)
- [Validation](Validation)
- [Caching](Caching)
- [UI](UI)
- [Samples](Samples)
- [Release Notes](Release-Notes)
