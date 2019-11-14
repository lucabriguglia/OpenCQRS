# Configuration

## Register services

- [Main](#main)
- [Store Provider](#store)
- [Message Bus Provider](#bus)
- [Validation Provider](#validation)
- [Caching Provider](#caching)
- [UI](#ui)

<a name="main"></a>
### Main

In ConfigureServices method of Startup.cs:

```C#
services.AddKledex(typeof(CreateProduct), typeof(GetProduct));
```

CreateProduct is an sample command and GetProduct is a sample query.
In this scenario, commands and queries are in two different assemblies.
Both assemblies need to be registered.

#### Options

It is possible to set a few options for the main package:

```C#
services
    .AddKledex(options =>
    {
         options.PublishEvents = true;
         options.SaveCommandData = true;
         options.ValidateCommands = false;
         options.CacheTime = 600;
     }, typeof(CreateProduct), typeof(GetProduct));
```

| Property | Description | Default | Notes |
| --- | --- | --- | --- |
| **PublishEvents** | The value indicating whether events are published automatically | true | The default behavior can be overridden by setting the PublishEvents property in any command |
| **SaveCommandData** | The value indicating whether domain commands data is saved | true | The default behavior can be overridden by setting the SaveCommandData property in any domain command |
| **ValidateCommands** | The value indicating whether all commands need to be validated before being sent to the handler | false | The default value can be overridden by setting the Validate property in any command |
| **CacheTime** | The value indicating the default cache time (in seconds) | 60 | The default value can be overridden by setting the CacheTime property in any cacheable query |

<a name="store"></a>
### Store

A domain store database provider needs to be registered as well in order to use the event sourcing functionalities.
After the NuGet package of your choice has been installed, register the database provider:

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddSqlServerProvider(Configuration);
```
#### Options

It is possible to set some options the CosmosDB providers.

**ComsosDB SQL API**

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddCosmosDbSqlProvider(Configuration, options =>
    {
        options.DatabaseId = "DatabaseId";
        options.AggregateCollectionId = "AggregateCollectionId";
        options.CommandCollectionId = "CommandCollectionId";
        options.EventCollectionId = "EventCollectionId";
    });
```

| Property | Description | Default |
| --- | --- | --- |
| **DatabaseId** | The Id of the Database | DomainStore |
| **AggregateCollectionId** | The Id of the Aggregate collection | Aggregates |
| **CommandCollectionId** | The Id of the Command collection | Commands |
| **EventCollectionId** | The Id of the Event collection | Events |

**ComsosDB Mongo API**

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddCosmosDbMongoProvider(Configuration, options =>
    {
        options.DatabaseName = "DatabaseName";
        options.AggregateCollectionName = "AggregateCollectionName";
        options.CommandCollectionName = "CommandCollectionName";
        options.EventCollectionName = "EventCollectionName";
    });
```

| Property | Description | Default |
| --- | --- | --- |
| **DatabaseName** | The Name of the Database | DomainStore |
| **AggregateCollectionName** | The Name of the Aggregate collection | Aggregates |
| **CommandCollectionName** | The Name of the Command collection | Commands |
| **EventCollectionName** | The Name of the Event collection | Events |

<a name="bus"></a>
### Message Bus

A message bus provider needs to be registered as well in order to use the message bus functionalities.
Kledex currently supports Azure Service Bus and RabbitMQ. After the NuGet package has been installed, register a provider:

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddSqlServerProvider(Configuration)
    .AddServiceBusProvider();
```

<a name="validation"></a>
### Validation

Add a validation provider if you want your commands to be validated before the command handler is executed:

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddSqlServerProvider(Configuration)
    .AddFluentValidationProvider();
```

<a name="caching"></a>
### Caching

...

<a name="ui"></a>
### UI

...

## Configure Services

In Configure method of Startup.cs:

For CosmosDB Sql (DocumentDB):

```C#
public void Configure(IApplicationBuilder app, IOptions<DomainDbConfiguration> settings)
{
    app.UseKledex().EnsureCosmosDbSqlDbCreated(settings);
}
```

For Entity Framework Core (SqlServer, MySQL, PostgreSQL and Sqlite):

```C#
public void Configure(IApplicationBuilder app)
{
    app.UseKledex().EnsureDomainDbCreated();
}
```

## App Settings

Update appsettings.json as required.

For any domain store provider or message bus provider and redis cache provider add the related connection string:

```JSON
"ConnectionStrings": {
  "KledexDomainStore": "Server=(localdb)\\mssqllocaldb;Database=DomainStore;Trusted_Connection=True;",
  "KledexMessageBus": "your-message-bus-connection-string",
  "KledexRedisCache": "your-redis-cache-connectionstring"
}
```

| Connection | For |
| --- | --- |
| **KledexDomainStore** | SqlServer, MySql, PostgreSql, Sqlite, Cosmos (Mongo) |
| **KledexMessageBus** | Service Bus, RabbitMQ |
| **KledexRedisCache** | Redis Cache |

Only for CosmosDB SQL (DocumentDB), add the following configuration section:

```JSON
{
  "KledexCosmosSqlConfiguration": {
    "ServerEndpoint": "https://localhost:8081",
    "AuthKey": "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
  }
}
```
