using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Weapsy.Mediator.Domain;
using Weapsy.Mediator.EventStore.CosmosDB.Sql.Configuration;
using Weapsy.Mediator.EventStore.CosmosDB.Sql.Documents;
using Weapsy.Mediator.EventStore.CosmosDB.Sql.Documents.Factories;

namespace Weapsy.Mediator.EventStore.CosmosDB.Sql
{
    internal class EventStore : IEventStore
    {
        private readonly IDocumentDbRepository<AggregateDocument> _aggregateRepository;
        private readonly IDocumentDbRepository<EventDocument> _eventRepository;
        private readonly IAggregateDocumentFactory _aggregateDocumentFactory;
        private readonly IEventDocumentFactory _eventDocumentFactory;
        private readonly string _aggregateCollectionId;
        private readonly string _eventCollectionId;

        public EventStore(IDocumentDbRepository<AggregateDocument> aggregateRepository, 
            IDocumentDbRepository<EventDocument> eventRepository,
            IAggregateDocumentFactory aggregateDocumentFactory,
            IEventDocumentFactory eventDocumentFactory,
            IOptions<CosmosDBSettings> settings)
        {
            _aggregateRepository = aggregateRepository;
            _eventRepository = eventRepository;
            _aggregateDocumentFactory = aggregateDocumentFactory;
            _eventDocumentFactory = eventDocumentFactory;
            _aggregateCollectionId = settings.Value.AggregateCollectionId;
            _eventCollectionId = settings.Value.EventCollectionId;
        }

        public async Task SaveEventAsync<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            var aggregateDocument = await _aggregateRepository.GetDocumentAsync(_aggregateCollectionId, @event.AggregateRootId.ToString());
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(@event);
                await _aggregateRepository.CreateDocumentAsync(_aggregateCollectionId, newAggregateDocument);
            }

            var currentVersion = await _eventRepository.GetCountAsync(_eventCollectionId, d => d.AggregateId == @event.AggregateRootId);
            var eventDocument = _eventDocumentFactory.CreateEvent(@event, currentVersion + 1);
            await _eventRepository.CreateDocumentAsync(_eventCollectionId, eventDocument);
        }

        public void SaveEvent<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            var aggregateDocument = _aggregateRepository.GetDocumentAsync(_aggregateCollectionId, @event.AggregateRootId.ToString()).GetAwaiter().GetResult();
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(@event);
                _aggregateRepository.CreateDocumentAsync(_aggregateCollectionId, newAggregateDocument).GetAwaiter().GetResult();
            }

            var currentVersion = _eventRepository.GetCountAsync(_eventCollectionId, d => d.AggregateId == @event.AggregateRootId).GetAwaiter().GetResult();
            var eventDocument = _eventDocumentFactory.CreateEvent(@event, currentVersion + 1);
            _eventRepository.CreateDocumentAsync(_eventCollectionId, eventDocument).GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId)
        {
            var result = new List<DomainEvent>();

            var events = await _eventRepository.GetDocumentsAsync(_eventCollectionId, d => d.AggregateId == aggregateId);

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

            var events = _eventRepository.GetDocumentsAsync(_eventCollectionId, d => d.AggregateId == aggregateId).GetAwaiter().GetResult();

            foreach (var @event in events)
            {
                var domainEvent = JsonConvert.DeserializeObject(@event.Data, Type.GetType(@event.Type));
                result.Add((DomainEvent)domainEvent);
            }

            return result;
        }
    }
}
