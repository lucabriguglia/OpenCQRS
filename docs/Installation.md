# Installation

## Installing the main package

Via Package Manager

    Install-Package Kledex
   
Or via .NET CLI

    dotnet add package Kledex
    
Or via Paket CLI

    paket add Kledex

## Installing a database provider package

Kledex currently supports the following database providers:

For CosmosDB:
- **Kledex.Store.Cosmos.Sql**
- **Kledex.Store.Cosmos.Mongo**

For Entity Framework Core:
- **Kledex.Store.EF.SqlServer**
- **Kledex.Store.EF.MySql**
- **Kledex.Store.EF.PostgreSql**
- **Kledex.Store.EF.Sqlite**

## Installing a message bus provider package

Kledex currently supports the following message bus providers:
- **Kledex.Bus.ServiceBus**
- **Kledex.Bus.RabbitMQ**

## Installing a validation provider package

Kledex currently supports the following validation providers:
- **Kledex.Validation.FluentValidation**

## Installing a caching provider package

Kledex currently supports the following caching providers:
- **Kledex.Caching.Memory**
- **Kledex.Caching.Redis**

## Installing the UI package

UI experimental package:
- **Kledex.UI**
