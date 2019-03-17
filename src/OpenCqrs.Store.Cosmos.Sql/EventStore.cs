using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenCqrs.Domain;
using OpenCqrs.Store.Cosmos.Sql.Documents;
using OpenCqrs.Store.Cosmos.Sql.Documents.Factories;
using OpenCqrs.Store.Cosmos.Sql.Repositories;

namespace OpenCqrs.Store.Cosmos.Sql
{
    internal class EventStore : IEventStore
    {
        private readonly IDocumentRepository<EventDocument> _eventRepository;
        private readonly IEventDocumentFactory _eventDocumentFactory;
        private readonly IVersionService _versionService;

        public EventStore(IDocumentRepository<EventDocument> eventRepository,
            IEventDocumentFactory eventDocumentFactory, 
            IVersionService versionService)
        {
            _eventRepository = eventRepository;
            _eventDocumentFactory = eventDocumentFactory;
            _versionService = versionService;
        }

        public async Task SaveEventAsync<TAggregate>(IDomainEvent @event, int? expectedVersion) where TAggregate : IAggregateRoot
        {
            var currentVersion = await _eventRepository.GetCountAsync(d => d.AggregateId == @event.AggregateRootId);
            var nextVersion = _versionService.GetNextVersion(@event.AggregateRootId, currentVersion, expectedVersion);

            var eventDocument = _eventDocumentFactory.CreateEvent(@event, nextVersion);

            await _eventRepository.CreateDocumentAsync(eventDocument);
        }

        public void SaveEvent<TAggregate>(IDomainEvent @event, int? expectedVersion) where TAggregate : IAggregateRoot
        {
            var currentVersion = _eventRepository.GetCountAsync(d => d.AggregateId == @event.AggregateRootId).GetAwaiter().GetResult();
            var nextVersion = _versionService.GetNextVersion(@event.AggregateRootId, currentVersion, expectedVersion);

            var eventDocument = _eventDocumentFactory.CreateEvent(@event, nextVersion);

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
