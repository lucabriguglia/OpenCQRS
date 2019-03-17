using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OpenCqrs.Domain;
using OpenCqrs.Store.Cosmos.Mongo.Configuration;
using OpenCqrs.Store.Cosmos.Mongo.Documents;
using OpenCqrs.Store.Cosmos.Mongo.Documents.Factories;

namespace OpenCqrs.Store.Cosmos.Mongo
{
    public class AggregateStore : IAggregateStore
    {
        private readonly DomainDbContext _dbContext;
        private readonly IAggregateDocumentFactory _aggregateDocumentFactory;

        public AggregateStore(IOptions<DomainDbConfiguration> settings, IAggregateDocumentFactory aggregateDocumentFactory)
        {
            _dbContext = new DomainDbContext(settings);
            _aggregateDocumentFactory = aggregateDocumentFactory;
        }

        public async Task SaveAggregateAsync<TAggregate>(Guid id) where TAggregate : IAggregateRoot
        {
            var aggregateFilter = Builders<AggregateDocument>.Filter.Eq("_id", id.ToString());
            var aggregateDocument = await _dbContext.Aggregates.Find(aggregateFilter).FirstOrDefaultAsync();
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(id);
                await _dbContext.Aggregates.InsertOneAsync(newAggregateDocument);
            }
        }

        public void SaveAggregate<TAggregate>(Guid id) where TAggregate : IAggregateRoot
        {
            var aggregateFilter = Builders<AggregateDocument>.Filter.Eq("_id", id.ToString());
            var aggregateDocument = _dbContext.Aggregates.Find(aggregateFilter).FirstOrDefault();
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(id);
                _dbContext.Aggregates.InsertOne(newAggregateDocument);
            }
        }
    }
}
