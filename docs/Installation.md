# Installation

Installing the various packages:

Via Package Manager

    Install-Package Kledex
   
Or via .NET CLI

    dotnet add package Kledex
    
Or via Paket CLI

    paket add Kledex

## Main package

- **Kledex**

## Database provider packages

Kledex currently supports the following database providers:

For CosmosDB:
- **Kledex.Store.Cosmos.Sql**
- **Kledex.Store.Cosmos.Mongo**

For Entity Framework Core:
- **Kledex.Store.EF.SqlServer**
- **Kledex.Store.EF.MySql**
- **Kledex.Store.EF.PostgreSql**
- **Kledex.Store.EF.Sqlite**

## Message bus provider packages

Kledex currently supports the following message bus providers:
- **Kledex.Bus.ServiceBus**
- **Kledex.Bus.RabbitMQ**

## Validation provider package

Kledex currently supports the following validation providers:
- **Kledex.Validation.FluentValidation**

## Caching provider packages

Kledex currently supports the following caching providers:
- **Kledex.Caching.Memory**
- **Kledex.Caching.Redis**