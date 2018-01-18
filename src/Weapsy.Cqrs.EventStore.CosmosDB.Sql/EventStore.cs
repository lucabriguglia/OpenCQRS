using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Weapsy.Cqrs.Domain;
using Weapsy.Cqrs.EventStore.CosmosDB.Sql.Configuration;
using Weapsy.Cqrs.EventStore.CosmosDB.Sql.Documents;
using Weapsy.Cqrs.EventStore.CosmosDB.Sql.Documents.Factories;

namespace Weapsy.Cqrs.EventStore.CosmosDB.Sql
{
    internal class EventStore : IEventStore
    {
        private readonly IDocumentDbRepository<AggregateDocument> _aggregateRepository;
        private readonly IDocumentDbRepository<EventDocument> _eventRepository;
        private readonly IAggregateDocumentFactory _aggregateDocumentFactory;
        private readonly IEventDocumentFactory _eventDocumentFactory;

        public EventStore(IDocumentDbRepository<AggregateDocument> aggregateRepository, 
            IDocumentDbRepository<EventDocument> eventRepository,
            IAggregateDocumentFactory aggregateDocumentFactory,
            IEventDocumentFactory eventDocumentFactory)
        {
            _aggregateRepository = aggregateRepository;
            _eventRepository = eventRepository;
            _aggregateDocumentFactory = aggregateDocumentFactory;
            _eventDocumentFactory = eventDocumentFactory;
        }

        public async Task SaveEventAsync<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            var aggregateDocument = await _aggregateRepository.GetDocumentAsync(@event.AggregateRootId.ToString());
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(@event);
                await _aggregateRepository.CreateDocumentAsync(newAggregateDocument);
            }

            var currentVersion = await _eventRepository.GetCountAsync(d => d.AggregateId == @event.AggregateRootId);
            var eventDocument = _eventDocumentFactory.CreateEvent(@event, currentVersion + 1);
            await _eventRepository.CreateDocumentAsync(eventDocument);
        }

        public void SaveEvent<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            var aggregateDocument = _aggregateRepository.GetDocumentAsync(@event.AggregateRootId.ToString()).GetAwaiter().GetResult();
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(@event);
                _aggregateRepository.CreateDocumentAsync(newAggregateDocument).GetAwaiter().GetResult();
            }

            var currentVersion = _eventRepository.GetCountAsync(d => d.AggregateId == @event.AggregateRootId).GetAwaiter().GetResult();
            var eventDocument = _eventDocumentFactory.CreateEvent(@event, currentVersion + 1);
            _eventRepository.CreateDocumentAsync(eventDocument).GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId)
        {
            var result = new List<DomainEvent>();

            var events = await _eventRepository.GetDocumentsAsync(d => d.AggregateId == aggregateId);

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

            var events = _eventRepository.GetDocumentsAsync(d => d.AggregateId == aggregateId).GetAwaiter().GetResult();

            foreach (var @event in events)
            {
                var domainEvent = JsonConvert.DeserializeObject(@event.Data, Type.GetType(@event.Type));
                result.Add((DomainEvent)domainEvent);
            }

            return result;
        }
    }
}
