# Kledex

[![Build Status](https://lucabriguglia.visualstudio.com/Kledex/_apis/build/status/lucabriguglia.Kledex?branchName=master)](https://lucabriguglia.visualstudio.com/Kledex/_build/latest?definitionId=1&branchName=master)

Kledex (formerly OpenCQRS) is a .NET Core framework that can be used to create a simple and clean design by enforcing single responsibility and separation of concerns.
Its advanced features are ideal for Domain Driven Design (DDD), Command Query Responsibilty Segragation (CQRS) and Event Sourcing.
Kledex also has Azure Service Bus and RabbitMQ integrations.

## Packages

| Package | Latest Stable |
| --- | --- |
| [Kledex](https://www.nuget.org/packages/Kledex) | [![Nuget Package](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/Kledex) |
| [Kledex.Store.Cosmos.Mongo](https://www.nuget.org/packages/Kledex.Store.Cosmos.Mongo) | [![Nuget Package](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.Cosmos.Mongo) |
| [Kledex.Store.Cosmos.Sql](https://www.nuget.org/packages/Kledex.Store.Cosmos.Sql) | [![Nuget Package](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.Cosmos.Sql) |
| [Kledex.Store.EF.MySql](https://www.nuget.org/packages/Kledex.Store.EF.MySql) | [![Nuget Package](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.EF.MySql) |
| [Kledex.Store.EF.PostgreSql](https://www.nuget.org/packages/Kledex.Store.EF.PostgreSql) | [![Nuget Package](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.EF.PostgreSql) |
| [Kledex.Store.EF.Sqlite](https://www.nuget.org/packages/Kledex.Store.EF.Sqlite) | [![Nuget Package](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.EF.Sqlite) |
| [Kledex.Store.EF.SqlServer](https://www.nuget.org/packages/Kledex.Store.EF.SqlServer) | [![Nuget Package](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.EF.SqlServer) |
| [Kledex.Store.EF.InMemory](https://www.nuget.org/packages/Kledex.Store.EF.InMemory) | [![Nuget Package](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.EF.InMemory) |
| [Kledex.Bus.ServiceBus](https://www.nuget.org/packages/Kledex.Bus.ServiceBus) | [![Nuget Package](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Bus.ServiceBus) |
| [Kledex.Bus.RabbitMQ](https://www.nuget.org/packages/Kledex.Bus.RabbitMQ) | [![Nuget Package](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Bus.RabbitMQ) |
| [Kledex.Validation.FluentValidation](https://www.nuget.org/packages/Kledex.Validation.FluentValidation) | [![Nuget Package](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Validation.FluentValidation) |
| [Kledex.Caching.Memory](https://www.nuget.org/packages/Kledex.Caching.Memory) | [![Nuget Package](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Caching.Memory) |
| [Kledex.Caching.Redis](https://www.nuget.org/packages/Kledex.Caching.Redis) | [![Nuget Package](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Caching.Redis) |
| [Kledex.UI](https://www.nuget.org/packages/Kledex.UI) | [![Nuget Package](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.UI) |

## Samples

Run the sample web projects to view how Kledex works and how it produces the same results with or without using event sourcing. The sample web applications use an experimental Kledex.UI package that returns a DTO containing the aggregate model with all events.

The following is a list of products created using the sample web application:

![List](https://github.com/lucabriguglia/Kledex/blob/master/docs/images/list.PNG)

And this is a detailed view of a product with all associated events:

![Product](https://github.com/lucabriguglia/Kledex/blob/master/docs/images/product.PNG)

## Resources

- [Wiki](https://github.com/lucabriguglia/Kledex/wiki)
- [Roadmap](https://github.com/lucabriguglia/Kledex/wiki/Roadmap)
