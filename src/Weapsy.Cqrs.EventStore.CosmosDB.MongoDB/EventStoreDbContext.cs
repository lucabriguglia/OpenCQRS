using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Weapsy.Cqrs.EventStore.CosmosDB.MongoDB.Configuration;
using Weapsy.Cqrs.EventStore.CosmosDB.MongoDB.Documents;

namespace Weapsy.Cqrs.EventStore.CosmosDB.MongoDB
{
    public class EventStoreDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly string _aggregateCollectionName;
        private readonly string _eventCollectionName;

        public EventStoreDbContext(IOptions<CosmosDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
            _aggregateCollectionName = settings.Value.AggregateCollectionName;
            _eventCollectionName = settings.Value.EventCollectionName;
        }

        public IMongoCollection<AggregateDocument> Aggregates => 
            _database.GetCollection<AggregateDocument>(_aggregateCollectionName);

        public IMongoCollection<EventDocument> Events =>
            _database.GetCollection<EventDocument>(_eventCollectionName);
    }
}