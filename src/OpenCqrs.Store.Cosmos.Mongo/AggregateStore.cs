using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OpenCqrs.Domain;
using OpenCqrs.Store.Cosmos.Mongo.Configuration;
using OpenCqrs.Store.Cosmos.Mongo.Documents;
using OpenCqrs.Store.Cosmos.Mongo.Documents.Factories;

namespace OpenCqrs.Store.Cosmos.Mongo
{
    /// <inheritdoc />
    public class AggregateStore : IAggregateStore
    {
        private readonly DomainDbContext _dbContext;
        private readonly IAggregateDocumentFactory _aggregateDocumentFactory;

        public AggregateStore(IOptions<DomainDbConfiguration> settings, IAggregateDocumentFactory aggregateDocumentFactory)
        {
            _dbContext = new DomainDbContext(settings);
            _aggregateDocumentFactory = aggregateDocumentFactory;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task<IEnumerable<AggregateStoreModel>> GetAggregatesAsync()
        {
            var result = new List<AggregateStoreModel>();

            var aggregateDocuments = await _dbContext.Aggregates.Find(_ => true).ToListAsync();

            foreach (var aggregateDocument in aggregateDocuments)
            {
                result.Add(new AggregateStoreModel
                {
                    Id = Guid.Parse(aggregateDocument.Id),
                    Type = aggregateDocument.Type
                });
            }

            return result;
        }

        /// <inheritdoc />
        public IEnumerable<AggregateStoreModel> GetAggregates()
        {
            var result = new List<AggregateStoreModel>();

            var aggregateDocuments = _dbContext.Aggregates.Find(_ => true).ToList();

            foreach (var aggregateDocument in aggregateDocuments)
            {
                result.Add(new AggregateStoreModel
                {
                    Id = Guid.Parse(aggregateDocument.Id),
                    Type = aggregateDocument.Type
                });
            }

            return result;
        }
    }
}
