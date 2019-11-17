using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Domain;
using Kledex.Store.Cosmos.Mongo.Documents;
using Kledex.Store.Cosmos.Mongo.Documents.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Kledex.Store.Cosmos.Mongo
{
    public class StoreProvider : IStoreProvider
    {
        private readonly DomainDbContext _dbContext;
        private readonly IAggregateDocumentFactory _aggregateDocumentFactory;
        private readonly ICommandDocumentFactory _commandDocumentFactory;
        private readonly IEventDocumentFactory _eventDocumentFactory;
        private readonly IVersionService _versionService;

        public StoreProvider(IConfiguration configuration, IOptions<DomainDbOptions> settings,
            IAggregateDocumentFactory aggregateDocumentFactory,
            ICommandDocumentFactory commandDocumentFactory,
            IEventDocumentFactory eventDocumentFactory,
            IVersionService versionService)
        {
            _dbContext = new DomainDbContext(configuration, settings);
            _aggregateDocumentFactory = aggregateDocumentFactory;
            _commandDocumentFactory = commandDocumentFactory;
            _commandDocumentFactory = commandDocumentFactory;
            _eventDocumentFactory = eventDocumentFactory;
            _versionService = versionService;
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

        public void Save(Type aggregateType, Guid aggregateRootId, IDomainCommand command, IEnumerable<IDomainEvent> events)
        {
            var aggregateFilter = Builders<AggregateDocument>.Filter.Eq("_id", aggregateRootId.ToString());
            var aggregateDocument = _dbContext.Aggregates.Find(aggregateFilter).FirstOrDefault();
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate(aggregateType, aggregateRootId);
                _dbContext.Aggregates.InsertOne(newAggregateDocument);
            }

            if (command != null)
            {
                var commandDocument = _commandDocumentFactory.CreateCommand(command);
                _dbContext.Commands.InsertOne(commandDocument);
            }

            foreach (var @event in events)
            {
                var eventFilter = Builders<EventDocument>.Filter.Eq("aggregateId", @event.AggregateRootId.ToString());
                var currentVersion = _dbContext.Events.Find(eventFilter).CountDocuments();
                var nextVersion = _versionService.GetNextVersion(@event.AggregateRootId, (int)currentVersion, command?.ExpectedVersion);

                var eventDocument = _eventDocumentFactory.CreateEvent(@event, nextVersion);

                _dbContext.Events.InsertOne(eventDocument);
            }
        }

        public async Task SaveAsync(Type aggregateType, Guid aggregateRootId, IDomainCommand command, IEnumerable<IDomainEvent> events)
        {
            var aggregateFilter = Builders<AggregateDocument>.Filter.Eq("_id", aggregateRootId.ToString());
            var aggregateDocument = await _dbContext.Aggregates.Find(aggregateFilter).FirstOrDefaultAsync();
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate(aggregateType, aggregateRootId);
                await _dbContext.Aggregates.InsertOneAsync(newAggregateDocument);
            }

            if (command != null)
            {
                var commandDocument = _commandDocumentFactory.CreateCommand(command);
                await _dbContext.Commands.InsertOneAsync(commandDocument);
            }

            foreach (var @event in events)
            {
                var eventFilter = Builders<EventDocument>.Filter.Eq("aggregateId", @event.AggregateRootId.ToString());
                var currentVersion = await _dbContext.Events.Find(eventFilter).CountDocumentsAsync();
                var nextVersion = _versionService.GetNextVersion(@event.AggregateRootId, (int)currentVersion, command?.ExpectedVersion);

                var eventDocument = _eventDocumentFactory.CreateEvent(@event, nextVersion);

                await _dbContext.Events.InsertOneAsync(eventDocument);
            }
        }
    }
}
