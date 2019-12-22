# Kledex

[![Build Status](https://lucabriguglia.visualstudio.com/Kledex/_apis/build/status/lucabriguglia.Kledex?branchName=master)](https://lucabriguglia.visualstudio.com/Kledex/_build/latest?definitionId=1&branchName=master)

Kledex is a .NET Core framework that can be used to create a simple and clean design by enforcing single responsibility and separation of concerns.

Its advanced features are ideal for Domain Driven Design (DDD), Command Query Responsibilty Segragation (CQRS) and Event Sourcing.

With Kledex you can automatically dispatch events to a message bus (Service Bus or RabbitMQ), validate your commands before they are sent to the command handler (FluentValidation) and automatically cache the result of your queries (Memory or Redis).

**Full Documentation**: [Kledex Wiki](https://lucabriguglia.github.io/Kledex).

**Visual Studio Template**: [DDD-CQRS-ES Starter Kit](https://github.com/lucabriguglia/DDD-CQRS-ES-Starter-Kit).

## Main Flow

![Send Command Flow](docs/assets/img/SendCommandFlow.svg)

## Packages

### Main

| Package | Latest Stable |
| --- | --- |
| [Kledex](https://www.nuget.org/packages/Kledex) | [![Nuget Package](https://img.shields.io/badge/nuget-2.4.0-blue.svg)](https://www.nuget.org/packages/Kledex) |

### Store Providers

| Package | Latest Stable |
| --- | --- |
| [Kledex.Store.Cosmos.Mongo](https://www.nuget.org/packages/Kledex.Store.Cosmos.Mongo) | [![Nuget Package](https://img.shields.io/badge/nuget-2.4.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.Cosmos.Mongo) |
| [Kledex.Store.Cosmos.Sql](https://www.nuget.org/packages/Kledex.Store.Cosmos.Sql) | [![Nuget Package](https://img.shields.io/badge/nuget-2.4.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.Cosmos.Sql) |
| [Kledex.Store.EF.MySql](https://www.nuget.org/packages/Kledex.Store.EF.MySql) | [![Nuget Package](https://img.shields.io/badge/nuget-2.4.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.EF.MySql) |
| [Kledex.Store.EF.PostgreSql](https://www.nuget.org/packages/Kledex.Store.EF.PostgreSql) | [![Nuget Package](https://img.shields.io/badge/nuget-2.4.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.EF.PostgreSql) |
| [Kledex.Store.EF.Sqlite](https://www.nuget.org/packages/Kledex.Store.EF.Sqlite) | [![Nuget Package](https://img.shields.io/badge/nuget-2.4.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.EF.Sqlite) |
| [Kledex.Store.EF.SqlServer](https://www.nuget.org/packages/Kledex.Store.EF.SqlServer) | [![Nuget Package](https://img.shields.io/badge/nuget-2.4.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.EF.SqlServer) |
| [Kledex.Store.EF.InMemory](https://www.nuget.org/packages/Kledex.Store.EF.InMemory) | [![Nuget Package](https://img.shields.io/badge/nuget-2.4.0-blue.svg)](https://www.nuget.org/packages/Kledex.Store.EF.InMemory) |

### Bus Providers

| Package | Latest Stable |
| --- | --- |
| [Kledex.Bus.ServiceBus](https://www.nuget.org/packages/Kledex.Bus.ServiceBus) | [![Nuget Package](https://img.shields.io/badge/nuget-2.4.0-blue.svg)](https://www.nuget.org/packages/Kledex.Bus.ServiceBus) |
| [Kledex.Bus.RabbitMQ](https://www.nuget.org/packages/Kledex.Bus.RabbitMQ) | [![Nuget Package](https://img.shields.io/badge/nuget-2.4.0-blue.svg)](https://www.nuget.org/packages/Kledex.Bus.RabbitMQ) |

### Validation Providers

| Package | Latest Stable |
| --- | --- |
| [Kledex.Validation.FluentValidation](https://www.nuget.org/packages/Kledex.Validation.FluentValidation) | [![Nuget Package](https://img.shields.io/badge/nuget-2.4.0-blue.svg)](https://www.nuget.org/packages/Kledex.Validation.FluentValidation) |

### Caching Providers

| Package | Latest Stable |
| --- | --- |
| [Kledex.Caching.Memory](https://www.nuget.org/packages/Kledex.Caching.Memory) | [![Nuget Package](https://img.shields.io/badge/nuget-2.4.0-blue.svg)](https://www.nuget.org/packages/Kledex.Caching.Memory) |
| [Kledex.Caching.Redis](https://www.nuget.org/packages/Kledex.Caching.Redis) | [![Nuget Package](https://img.shields.io/badge/nuget-2.4.0-blue.svg)](https://www.nuget.org/packages/Kledex.Caching.Redis) |

### Misc

| Package | Latest Stable |
| --- | --- |
| [Kledex.UI](https://www.nuget.org/packages/Kledex.UI) | [![Nuget Package](https://img.shields.io/badge/nuget-2.4.0-blue.svg)](https://www.nuget.org/packages/Kledex.UI) |
