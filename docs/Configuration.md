## Register services

In ConfigureServices method of Startup.cs:

```C#
services.AddKledex(typeof(CreateProduct), typeof(GetProduct));
```

CreateProduct is an sample command and GetProduct is a sample query.
In this scenario, commands and queries are in two different assemblies.
Both assemblies need to be registered.

A domain store database provider needs to be registered as well in order to use the event sourcing functionalities.
After the NuGet package of your choice has been installed, register the database provider:

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddSqlServerProvider(Configuration);
```

You can also set a few options:

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddOptions(opt =>
    {
        opt.PublishEvents = true;
        opt.SaveCommandData = false;
    })
    .AddSqlServerProvider(Configuration);
```

**PublishEvents**
- The value indicating whether events are published automatically. 
- Default value is true.
- The default behavior can be overridden by setting the PublishEvents in any command.

**SaveCommandData**
- The value indicating whether commands are saved alongside events. 
- Default value is true.
- The default behavior can be overridden by setting the SaveCommandData in any domain command.

A message bus provider needs to be registered as well in order to use the message bus functionalities.
Kledex currently supports Azure Service Bus and RabbitMQ. After the NuGet package has been installed, register a provider:

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddSqlServerProvider(Configuration)
    .AddServiceBusProvider(Configuration);
```

Add a validation provider if you want your commands to be validated before the command handler is called:

```C#
services
    .AddKledex(typeof(CreateProduct), typeof(GetProduct))
    .AddSqlServerProvider(Configuration)
    .AddServiceBusProvider(Configuration)
    .AddFluentValidationProvider();
```

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

## Settings

Update appsettings.json with the following configurations.

For CosmosDB SQL (DocumentDB):

```JSON
{
  "DomainDbConfiguration": {
    "ServerEndpoint": "https://localhost:8081",
    "AuthKey": "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
    "DatabaseId": "DomainStore",
    "AggregateCollectionId": "Aggregates",
    "CommandCollectionId": "Commands",
    "EventCollectionId": "Events"
  }
}
```

For CosmosDB MongoDB:

```JSON
{
  "DomainDbConfiguration": {
    "ConnectionString": "mongodb://localhost:C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==@localhost:10255/admin?ssl=true",
    "DatabaseName": "DomainStore",
    "AggregateCollectionName": "Aggregates",
    "CommandCollectionName": "Commands",
    "EventCollectionName": "Events"
  }
}
```

For Entity Framework Core (SqlServer, MySQL, PostgreSQL and Sqlite):

```JSON
{
  "DomainDbConfiguration": {
    "ConnectionString": "Server=(localdb)\\mssqllocaldb;Database=DomainDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

For Azure Service Bus:

```JSON
{
  "ServiceBusConfiguration": {
    "ConnectionString": "your-azure-service-bus-connection-string"
  }
}
```

For RabbitMQ:

```JSON
{
  "RabbitMQConfiguration": {
    "ConnectionString": "your-rabbit-mq-connection-string"
  }
}
```