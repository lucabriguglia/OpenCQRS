using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using OpenCqrs.Abstractions.Domain;
using OpenCqrs.Domain;
using OpenCqrs.Store.Cosmos.Mongo.Configuration;
using OpenCqrs.Store.Cosmos.Mongo.Documents;
using OpenCqrs.Store.Cosmos.Mongo.Documents.Factories;

namespace OpenCqrs.Store.Cosmos.Mongo
{
    public class EventStore : IEventStore
    {
        private readonly DomainDbContext _dbContext;
        private readonly IAggregateDocumentFactory _aggregateDocumentFactory;
        private readonly IEventDocumentFactory _eventDocumentFactory;
        private readonly IVersionService _versionService;

        public EventStore(IOptions<DomainDbConfiguration> settings, 
            IAggregateDocumentFactory aggregateDocumentFactory, 
            IEventDocumentFactory eventDocumentFactory, 
            IVersionService versionService)
        {
            _dbContext = new DomainDbContext(settings);
            _aggregateDocumentFactory = aggregateDocumentFactory;
            _eventDocumentFactory = eventDocumentFactory;
            _versionService = versionService;
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
            var nextVersion = _versionService.GetNextVersion(@event.AggregateRootId, (int)currentVersion, expectedVersion);

            var eventDocument = _eventDocumentFactory.CreateEvent(@event, nextVersion);

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
            var nextVersion = _versionService.GetNextVersion(@event.AggregateRootId, (int)currentVersion, expectedVersion);

            var eventDocument = _eventDocumentFactory.CreateEvent(@event, nextVersion);

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
