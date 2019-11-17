# Providers

It is possible to customise Kledex by creating a custom provider.
Thre are four different providers and for each of them there is only one interface to implement in order to create a custom one.

| Provider | Description | Interface | Example
| --- | --- | --- | --- |
| **Store** | Stores domain data | _IStoreProvider_ | [CosmosDB Mongo API](https://github.com/lucabriguglia/Kledex/tree/master/src/Kledex.Store.Cosmos.Mongo) |
| **Bus** | Sends events to a message bus | _IBusProvider_ | [RabbitMQ](https://github.com/lucabriguglia/Kledex/tree/master/src/Kledex.Bus.RabbitMQ) |
| **Caching** | Gets query results from cache | _ICacheProvider_ | [Redis](https://github.com/lucabriguglia/Kledex/tree/master/src/Kledex.Cache.Redis) |
| **Validation** | Validates commands | _IValidationProvider_ | [FluentValidation](https://github.com/lucabriguglia/Kledex/tree/master/src/Kledex.Validation.FluentValidation) |
