# OpenCQRS

[![Build status](https://ci.appveyor.com/api/projects/status/p5p80y0fa6e9wbaa?svg=true)](https://ci.appveyor.com/project/lucabriguglia/opencqrs)

OpenCQRS is a .NET Core framework for Domain Driven Design (DDD), Command Query Responsibilty Segragation (CQRS) and Event Sourcing with Azure Service Bus integration.

## New

- OpenCqrs.Bus.Rabbitmq supported
- OpenCqrs.Abstractions: This assembly allows having few depencies in Domain
- Rabbitmq enlistment transactions supported. This allows atomic commits in Database & Message Bus
- Azure Service Bus doesn't support DTC or 2PC (Two-phase Commit). I recommend using Rabbitmq to avoid possible inconsistencies.

## Nuget Packages

[![Nuget Package](https://img.shields.io/badge/OpenCqrs-5.3.0-blue.svg)](https://www.nuget.org/packages/OpenCqrs)

[![Nuget Package](https://img.shields.io/badge/OpenCqrs.Store.Cosmos.Mongo-5.3.0-blue.svg)](https://www.nuget.org/packages/OpenCqrs.Store.Cosmos.Mongo)

[![Nuget Package](https://img.shields.io/badge/OpenCqrs.Store.Cosmos.Sql-5.3.0-blue.svg)](https://www.nuget.org/packages/OpenCqrs.Store.Cosmos.Sql)

[![Nuget Package](https://img.shields.io/badge/OpenCqrs.Store.EF.MySql-5.3.0-blue.svg)](https://www.nuget.org/packages/OpenCqrs.Store.EF.MySql)

[![Nuget Package](https://img.shields.io/badge/OpenCqrs.Store.EF.PostgreSql-5.3.0-blue.svg)](https://www.nuget.org/packages/OpenCqrs.Store.EF.PostgreSql)

[![Nuget Package](https://img.shields.io/badge/OpenCqrs.Store.EF.Sqlite-5.3.0-blue.svg)](https://www.nuget.org/packages/OpenCqrs.Store.EF.Sqlite)

[![Nuget Package](https://img.shields.io/badge/OpenCqrs.Store.EF.SqlServer-5.3.0-blue.svg)](https://www.nuget.org/packages/OpenCqrs.Store.EF.SqlServer)

[![Nuget Package](https://img.shields.io/badge/OpenCqrs.Bus.ServiceBus-5.3.0-blue.svg)](https://www.nuget.org/packages/OpenCqrs.Bus.ServiceBus)

## Wiki

https://github.com/OpenCQRS/OpenCQRS/wiki

## Roadmap

https://github.com/OpenCQRS/OpenCQRS/wiki/Roadmap
