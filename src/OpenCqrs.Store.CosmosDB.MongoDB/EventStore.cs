using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using OpenCqrs.Domain;
using OpenCqrs.Exceptions;
using OpenCqrs.Store.CosmosDB.MongoDB.Configuration;
using OpenCqrs.Store.CosmosDB.MongoDB.Documents;
using OpenCqrs.Store.CosmosDB.MongoDB.Documents.Factories;

namespace OpenCqrs.Store.CosmosDB.MongoDB
{
    public class EventStore : IEventStore
    {
        private readonly DomainDbContext _dbContext;
        private readonly IAggregateDocumentFactory _aggregateDocumentFactory;
        private readonly IEventDocumentFactory _eventDocumentFactory;

        public EventStore(IOptions<DomainDbConfiguration> settings, 
            IAggregateDocumentFactory aggregateDocumentFactory, 
            IEventDocumentFactory eventDocumentFactory)
        {
            _dbContext = new DomainDbContext(settings);
            _aggregateDocumentFactory = aggregateDocumentFactory;
            _eventDocumentFactory = eventDocumentFactory;
        }

        public async Task SaveEventAsync<TAggregate>(IDomainEvent @event, int? expectedVersion) where TAggregate : IAggregateRoot
        {
            var aggregateFilter = Builders<AggregateDocument>.Filter.Eq("_id", @event.AggregateRootId.ToString());
            var aggregate = await _dbContext.Aggregates.Find(aggregateFilter).FirstOrDefaultAsync();
            if (aggregate == null)
            {
                var aggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(@event.AggregateRootId);
                await _dbContext.Aggregates.InsertOneAsync(aggregateDocument);
            }

            var eventFilter = Builders<EventDocument>.Filter.Eq("aggregateId", @event.AggregateRootId.ToString());
            var currentVersion = await _dbContext.Events.Find(eventFilter).CountDocumentsAsync();

            if (expectedVersion.HasValue && expectedVersion.Value > 0 && expectedVersion.Value != currentVersion)
                throw new ConcurrencyException(@event.AggregateRootId, expectedVersion.Value, (int)currentVersion);

            var eventDocument = _eventDocumentFactory.CreateEvent(@event, currentVersion + 1);

            await _dbContext.Events.InsertOneAsync(eventDocument);
        }

        public void SaveEvent<TAggregate>(IDomainEvent @event, int? expectedVersion) where TAggregate : IAggregateRoot
        {
            var aggregateFilter = Builders<AggregateDocument>.Filter.Eq("_id", @event.AggregateRootId.ToString());
            var aggregate = _dbContext.Aggregates.Find(aggregateFilter).FirstOrDefault();
            if (aggregate == null)
            {
                var aggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(@event.AggregateRootId);
                _dbContext.Aggregates.InsertOne(aggregateDocument);
            }

            var eventFilter = Builders<EventDocument>.Filter.Eq("aggregateId", @event.AggregateRootId.ToString());
            var currentVersion = _dbContext.Events.Find(eventFilter).CountDocuments();

            if (expectedVersion.HasValue && expectedVersion.Value > 0 && expectedVersion.Value != currentVersion)
                throw new ConcurrencyException(@event.AggregateRootId, expectedVersion.Value, (int)currentVersion);

            var eventDocument = _eventDocumentFactory.CreateEvent(@event, currentVersion + 1);

            _dbContext.Events.InsertOne(eventDocument);
        }

        public async Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId)
        {
            var result = new List<DomainEvent>();

            var filter = Builders<EventDocument>.Filter.Eq("aggregateId", aggregateId.ToString());
            var events = await _dbContext.Events.Find(filter).ToListAsync();

            foreach (var @event in events)
            {
                var domainEvent = JsonConvert.DeserializeObject(@event.Data, Type.GetType(@event.Type));
                result.Add((DomainEvent)domainEvent);
            }

            return result;
        }

        public IEnumerable<DomainEvent> GetEvents(Guid aggregateId)
        {
            var result = new List<DomainEvent>();

            var filter = Builders<EventDocument>.Filter.Eq("aggregateId", aggregateId.ToString());
            var events = _dbContext.Events.Find(filter).ToList();

            foreach (var @event in events)
            {
                var domainEvent = JsonConvert.DeserializeObject(@event.Data, Type.GetType(@event.Type));
                result.Add((DomainEvent)domainEvent);
            }

            return result;
        }
    }
}
