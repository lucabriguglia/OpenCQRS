# Configuration

- [Register Services](#register)
    - [Main](#main)
    - [Store Provider](#store)
    - [Message Bus Provider](#bus)
    - [Validation Provider](#validation)
    - [Caching Provider](#caching)
    - [UI](#ui)
- [Configure Services](#configure)
    - [CosmosDB SQL API](#config-cosmos)
    - [Entity Framework Core](#config-ef)
- [App Settings](#settings)
    - [Connection Strings](#settings-connstrings)
    - [CosmosDB SQL API](#settings-cosmos)

<a name="register"></a>
## Register services

<a name="main"></a>
### Main

In ConfigureServices method of Startup.cs:

```C#
services.AddKledex(typeof(CreateProduct), typeof(GetProduct));
```

You need to pass only one type per assembly that contains your command, event, query and validation handlers.
CreateProduct is an sample command and GetProduct is a sample query.
In this scenario, commands and queries are in two different assemblies.
Both assemblies need to be registered.

It is possible to set a few options for the main package:

```C#
services
    .AddKledex(options =>
    {
         options.PublishEvents = true;
         options.SaveCommandData = true;
     }, typeof(CreateProduct), typeof(GetProduct));
```

| Property | Description | Default | Notes |
| --- | --- | --- | --- |
| **PublishEvents** | The value indicating whether events are published automatically | true | The default behavior can be overridden by setting the PublishEvents property in any command |
| **SaveCommandData** | The value indicating whether domain commands data is saved | true | The default behavior can be overridden by setting the SaveCommandData property in any domain command |

<a name="store"></a>
### Store

A domain store database provider needs to be registered in order to use the event sourcing functionalities.
After the NuGet package of your choice has been installed, register the database provider:

| Package | Method |
| --- | --- |
| **Kledex.Store.Cosmos.Mongo** | AddCosmosMongo |
| **Kledex.Store.Cosmos.Sql** | AddCosmosSql |
| **Kledex.Store.EF.MySql** | AddMySql |
| **Kledex.Store.EF.PostgreSql** | AddPostgreSql |
| **Kledex.Store.EF.Sqlite** | AddSqlite |
| **Kledex.Store.EF.SqlServer** | AddSqlServer |
| **Kledex.Store.EF.Cosmos** | AddCosmos |

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddSqlServer(options =>
    {
        options.ConnectionString = "your-connection-string";
    })
```

Each store provider has got some options, some of them required such as the connection string.

**EF.MySql, EF.PostgreSql, EF.Sqlite and EF.SqlServer**

| Property | Description | Default |
| --- | --- | --- | --- |
| **ConnectionString** | The connection string of the database | null |

**EF.Cosmos**

This provider uses the new Microsoft.Azure.Cosmos package.

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddCosmos(options =>
    {
        options.ServiceEndpoint = "your-service-end-point";
        options.AuthKey = "your-auth-key";
	options.DatabaseName = "DatabaseId";
	options.AggregateContainerName = "Aggregates";
	options.CommandContainerName = "Commands";
	options.EventContainerName = "Events";
    });
```

| Property | Description | Default |
| --- | --- | --- |
| **ServiceEndpoint** | The name of the Database | https://localhost:8081 |
| **AuthKey** | The name of the Database | C2y6yDjf5/R+ob0N8A7Cgv30VRDJ... |
| **DatabaseName** | The name of the Database | DomainStore |
| **AggregateContainerName** | The name of the Aggregate container | Aggregates |
| **CommandContainerName** | The name of the Command container | Commands |
| **EventContainerName** | The name of the Event container | Events |

**Cosmos.Sql**

This provider uses the old Microsoft.Azure.DocumentDB package.

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddCosmosSql(options =>
    {
        options.ServiceEndpoint = "your-service-end-point";
        options.AuthKey = "your-auth-key";
	options.DatabaseId = "DatabaseId";
	options.AggregateCollectionId = "AggregateCollectionId";
	options.CommandCollectionId = "CommandCollectionId";
	options.EventCollectionId = "EventCollectionId";
	options.OfferThroughput = 400;
	options.ConsistencyLevel = ConsistencyLevel.Session;
    });
```

| Property | Description | Default |
| --- | --- | --- |
| **DatabaseId** | The Id of the Database | DomainStore |
| **AggregateCollectionId** | The Id of the Aggregate collection | Aggregates |
| **CommandCollectionId** | The Id of the Command collection | Commands |
| **EventCollectionId** | The Id of the Event collection | Events |
| **OfferThroughput** | The offer throughput provisioned | 400 |
| **ConsistencyLevel** | The consistency level | Session |

Note that the partition key is set by default to '/type'.

**Cosmos.Mongo**

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddCosmosMongo(options =>
    {
        options.ConnectionString = "your-connection-string";
        options.DatabaseName = "DatabaseName";
        options.AggregateCollectionName = "AggregateCollectionName";
        options.CommandCollectionName = "CommandCollectionName";
        options.EventCollectionName = "EventCollectionName";
    });
```

| Property | Description | Default |
| --- | --- | --- |
| **ConnectionString** | The connection string of the database | null |
| **DatabaseName** | The Name of the Database | DomainStore |
| **AggregateCollectionName** | The Name of the Aggregate collection | Aggregates |
| **CommandCollectionName** | The Name of the Command collection | Commands |
| **EventCollectionName** | The Name of the Event collection | Events |

<a name="bus"></a>
### Message Bus

A message bus provider needs to be registered as well in order to use the message bus functionalities.
Kledex currently supports Azure Service Bus and RabbitMQ. After the NuGet package has been installed, register a provider:

| Package | Method |
| --- | --- |
| **Kledex.Bus.RabbitMQ** | AddRabbitMQ |
| **Kledex.Bus.ServiceBus** | AddServiceBus |

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddCosmosStore()
    .AddServiceBus();
```

<a name="validation"></a>
### Validation

Add a validation provider if you want your commands to be validated before the command handler is executed:

| Package | Method |
| --- | --- |
| **Kledex.Validation.FluentValidation** | AddFluentValidation |

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddCosmosStore()
    .AddFluentValidation();
```

| Property | Description | Default | Notes |
| --- | --- | --- | --- |
| **ValidateAllCommands** | The value indicating whether all commands need to be validated before being sent to the handler | false | The default value can be overridden by setting the Validate property in any command |

<a name="caching"></a>
### Caching

Add a caching provider if you want the result of your queries to be automatically cached:

| Package | Method |
| --- | --- |
| **Kledex.Caching.Memory** | AddMemoryCache |
| **Kledex.Caching.Redis** | AddRedisCache |

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddCosmosStore()
    .AddMemoryCache();
```

| Property | Description | Default | Notes |
| --- | --- | --- | --- |
| **DefaultCacheTime** | The value indicating the default cache time (in seconds) | 60 | The default value can be overridden by setting the CacheTime property in any cacheable query |

<a name="ui"></a>
### UI

Experimental package to get a view of an aggregate and all associated events:

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddCosmosStore()
    .AddUI();
```

<a name="configure"></a>
## Configure Services

In Configure method of Startup.cs:

<a name="config-cosmos"></a>
### CosmosDB SQL API

For CosmosDB SQL API:

```C#
public void Configure(IApplicationBuilder app, IOptions<DomainDbConfiguration> settings)
{
    app.UseKledex().EnsureCosmosDbSqlDbCreated(settings);
}
```

<a name="config-ef"></a>
### Entity Framework Core

For Entity Framework Core (SqlServer, MySQL, PostgreSQL and Sqlite):

```C#
public void Configure(IApplicationBuilder app)
{
    app.UseKledex().EnsureDomainDbCreated();
}
```

<a name="settings"></a>
## App Settings

Update appsettings.json as required.

<a name="settings-connstrings"></a>
### Connection Strings

For any domain store provider or message bus provider and redis cache provider add the related connection string:

```JSON
"ConnectionStrings": {
  "KledexDomainStore": "Server=(localdb)\\mssqllocaldb;Database=DomainStore;Trusted_Connection=True;",
  "KledexMessageBus": "your-message-bus-connection-string",
  "KledexRedisCache": "your-redis-cache-connectionstring"
}
```

| Name | For |
| --- | --- |
| **KledexDomainStore** | SqlServer, MySql, PostgreSql, Sqlite, CosmosDB Mongo API |
| **KledexMessageBus** | Service Bus, RabbitMQ |
| **KledexRedisCache** | Redis Cache |

<a name="settings-cosmos"></a>
### CosmosDB SQL API

Only for CosmosDB SQL API, add the following configuration section:

```JSON
{
  "KledexCosmosSqlConfiguration": {
    "ServerEndpoint": "https://localhost:8081",
    "AuthKey": "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
  }
}
```
