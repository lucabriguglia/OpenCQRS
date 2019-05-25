using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Domain;
using Kledex.Store.Cosmos.Mongo.Configuration;
using Kledex.Store.Cosmos.Mongo.Documents;
using Kledex.Store.Cosmos.Mongo.Documents.Factories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Kledex.Store.Cosmos.Mongo
{
    /// <inheritdoc />
    public class EventStore : IEventStore
    {
        private readonly DomainDbContext _dbContext;
        private readonly IEventDocumentFactory _eventDocumentFactory;
        private readonly IVersionService _versionService;

        public EventStore(IOptions<DomainDbConfiguration> settings, 
            IEventDocumentFactory eventDocumentFactory, 
            IVersionService versionService)
        {
            _dbContext = new DomainDbContext(settings);
            _eventDocumentFactory = eventDocumentFactory;
            _versionService = versionService;
        }

        /// <inheritdoc />
        public async Task SaveEventAsync<TAggregate>(IDomainEvent @event, int? expectedVersion) where TAggregate : IAggregateRoot
        {
            var eventFilter = Builders<EventDocument>.Filter.Eq("aggregateId", @event.AggregateRootId.ToString());
            var currentVersion = await _dbContext.Events.Find(eventFilter).CountDocumentsAsync();
            var nextVersion = _versionService.GetNextVersion(@event.AggregateRootId, (int)currentVersion, expectedVersion);

            var eventDocument = _eventDocumentFactory.CreateEvent(@event, nextVersion);

            await _dbContext.Events.InsertOneAsync(eventDocument);
        }

        /// <inheritdoc />
        public void SaveEvent<TAggregate>(IDomainEvent @event, int? expectedVersion) where TAggregate : IAggregateRoot
        {
            var eventFilter = Builders<EventDocument>.Filter.Eq("aggregateId", @event.AggregateRootId.ToString());
            var currentVersion = _dbContext.Events.Find(eventFilter).CountDocuments();
            var nextVersion = _versionService.GetNextVersion(@event.AggregateRootId, (int)currentVersion, expectedVersion);

            var eventDocument = _eventDocumentFactory.CreateEvent(@event, nextVersion);

            _dbContext.Events.InsertOne(eventDocument);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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
