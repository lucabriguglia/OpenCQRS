using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenCqrs.Abstractions.Domain;
using OpenCqrs.Domain;
using OpenCqrs.Store.Cosmos.Sql.Documents;
using OpenCqrs.Store.Cosmos.Sql.Documents.Factories;
using OpenCqrs.Store.Cosmos.Sql.Repositories;

namespace OpenCqrs.Store.Cosmos.Sql
{
    internal class EventStore : IEventStore
    {
        private readonly IDocumentRepository<AggregateDocument> _aggregateRepository;
        private readonly IDocumentRepository<EventDocument> _eventRepository;
        private readonly IAggregateDocumentFactory _aggregateDocumentFactory;
        private readonly IEventDocumentFactory _eventDocumentFactory;
        private readonly IVersionService _versionService;

        public EventStore(IDocumentRepository<AggregateDocument> aggregateRepository, 
            IDocumentRepository<EventDocument> eventRepository,
            IAggregateDocumentFactory aggregateDocumentFactory,
            IEventDocumentFactory eventDocumentFactory, 
            IVersionService versionService)
        {
            _aggregateRepository = aggregateRepository;
            _eventRepository = eventRepository;
            _aggregateDocumentFactory = aggregateDocumentFactory;
            _eventDocumentFactory = eventDocumentFactory;
            _versionService = versionService;
        }

        public async Task SaveEventAsync<TAggregate>(IDomainEvent @event, int? expectedVersion) where TAggregate : IAggregateRoot
        {
            var aggregateDocument = await _aggregateRepository.GetDocumentAsync(@event.AggregateRootId.ToString());
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(@event.AggregateRootId);
                await _aggregateRepository.CreateDocumentAsync(newAggregateDocument);
            }

            var currentVersion = await _eventRepository.GetCountAsync(d => d.AggregateId == @event.AggregateRootId);
            var nextVersion = _versionService.GetNextVersion(@event.AggregateRootId, currentVersion, expectedVersion);

            var eventDocument = _eventDocumentFactory.CreateEvent(@event, nextVersion);

            await _eventRepository.CreateDocumentAsync(eventDocument);
        }

        public void SaveEvent<TAggregate>(IDomainEvent @event, int? expectedVersion) where TAggregate : IAggregateRoot
        {
            var aggregateDocument = _aggregateRepository.GetDocumentAsync(@event.AggregateRootId.ToString()).GetAwaiter().GetResult();
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(@event.AggregateRootId);
                _aggregateRepository.CreateDocumentAsync(newAggregateDocument).GetAwaiter().GetResult();
            }

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
