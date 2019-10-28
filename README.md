# Kledex (formerly OpenCQRS)

[![Build Status](https://lucabriguglia.visualstudio.com/Kledex/_apis/build/status/lucabriguglia.Kledex?branchName=master)](https://lucabriguglia.visualstudio.com/Kledex/_build/latest?definitionId=1&branchName=master)

Kledex is a .NET Core framework that can be used to create a simple and clean design by enforcing single responsibility and separation of concerns.
Its advanced features are ideal for Domain Driven Design (DDD), Command Query Responsibilty Segragation (CQRS) and Event Sourcing.
Kledex also has Azure Service Bus and RabbitMQ integrations.

## Nuget Packages

### Main

[![Nuget Package](https://img.shields.io/badge/Kledex-1.2.0-blue.svg)](https://www.nuget.org/packages/Kledex)

### Storage

[![Nuget Package](https://img.shields.io/badge/Kledex.Store.Cosmos.Mongo-1.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.Cosmos.Mongo)

[![Nuget Package](https://img.shields.io/badge/Kledex.Store.Cosmos.Sql-1.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.Cosmos.Sql)

[![Nuget Package](https://img.shields.io/badge/Kledex.Store.EF.MySql-1.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.EF.MySql)

[![Nuget Package](https://img.shields.io/badge/Kledex.Store.EF.PostgreSql-1.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.EF.PostgreSql)

[![Nuget Package](https://img.shields.io/badge/Kledex.Store.EF.Sqlite-1.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.EF.Sqlite)

[![Nuget Package](https://img.shields.io/badge/Kledex.Store.EF.SqlServer-1.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.EF.SqlServer)

[![Nuget Package](https://img.shields.io/badge/Kledex.Store.EF.InMemory-1.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.EF.InMemory)

### Bus

[![Nuget Package](https://img.shields.io/badge/Kledex.Bus.ServiceBus-1.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Bus.ServiceBus)

[![Nuget Package](https://img.shields.io/badge/Kledex.Bus.RabbitMQ-1.2.0-blue.svg)](https://www.nuget.org/packages/Kledex.Bus.RabbitMQ)

## Samples

Run the sample web projects to view how Kledex works and how it produces the same results with or without using event sourcing. The sample web applications use an experimental Kledex.UI package that returns a DTO containing the aggragate model with all events.

The following is a list of products created using the sample web application:

![List](https://github.com/lucabriguglia/Kledex/blob/master/docs/images/list.PNG)

And this is a detailed view of a product with all associated events:

![Product](https://github.com/lucabriguglia/Kledex/blob/master/docs/images/product.PNG)

## Resources

- [Wiki](https://github.com/lucabriguglia/Kledex/wiki)
- [Roadmap](https://github.com/lucabriguglia/Kledex/wiki/Roadmap)
