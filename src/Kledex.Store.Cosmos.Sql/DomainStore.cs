using Kledex.Domain;
using Kledex.Store.Cosmos.Sql.Documents;
using Kledex.Store.Cosmos.Sql.Documents.Factories;
using Kledex.Store.Cosmos.Sql.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kledex.Store.Cosmos.Sql
{
    public class DomainStore : IDomainStore
    {
        private readonly IDocumentRepository<AggregateDocument> _aggregateRepository;
        private readonly IDocumentRepository<CommandDocument> _commandRepository;
        private readonly IDocumentRepository<EventDocument> _eventRepository;
        private readonly IAggregateDocumentFactory _aggregateDocumentFactory;
        private readonly ICommandDocumentFactory _commandDocumentFactory;
        private readonly IEventDocumentFactory _eventDocumentFactory;
        private readonly IVersionService _versionService;

        public DomainStore(IDocumentRepository<AggregateDocument> aggregateRepository,
            IDocumentRepository<CommandDocument> commandRepository,
            IDocumentRepository<EventDocument> eventRepository,
            IAggregateDocumentFactory aggregateDocumentFactory,
            ICommandDocumentFactory commandDocumentFactory,
            IEventDocumentFactory eventDocumentFactory,
            IVersionService versionService)
        {
            _aggregateRepository = aggregateRepository;
            _commandRepository = commandRepository;
            _eventRepository = eventRepository;
            _aggregateDocumentFactory = aggregateDocumentFactory;
            _commandDocumentFactory = commandDocumentFactory;
            _commandDocumentFactory = commandDocumentFactory;
            _eventDocumentFactory = eventDocumentFactory;
            _versionService = versionService;
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

        public void Save<TAggregate>(Guid aggregateRootId, IDomainCommand command, IEnumerable<IDomainEvent> events) where TAggregate : IAggregateRoot
        {
            var aggregateDocument = _aggregateRepository.GetDocumentAsync(aggregateRootId.ToString()).GetAwaiter().GetResult();
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(aggregateRootId);
                _aggregateRepository.CreateDocumentAsync(newAggregateDocument).GetAwaiter().GetResult();
            }

            var commandDocument = _commandDocumentFactory.CreateCommand(command);
            _commandRepository.CreateDocumentAsync(commandDocument).GetAwaiter().GetResult();

            foreach (var @event in events)
            {
                var currentVersion = _eventRepository.GetCountAsync(d => d.AggregateId == @event.AggregateRootId).GetAwaiter().GetResult();
                var nextVersion = _versionService.GetNextVersion(@event.AggregateRootId, currentVersion, command?.ExpectedVersion);

                var eventDocument = _eventDocumentFactory.CreateEvent(@event, nextVersion);

                _eventRepository.CreateDocumentAsync(eventDocument).GetAwaiter().GetResult();
            }
        }

        public async Task SaveAsync<TAggregate>(Guid aggregateRootId, IDomainCommand command, IEnumerable<IDomainEvent> events) where TAggregate : IAggregateRoot
        {
            var aggregateDocument = await _aggregateRepository.GetDocumentAsync(aggregateRootId.ToString());
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(aggregateRootId);
                await _aggregateRepository.CreateDocumentAsync(newAggregateDocument);
            }

            var commandDocument = _commandDocumentFactory.CreateCommand(command);
            await _commandRepository.CreateDocumentAsync(commandDocument);

            foreach (var @event in events)
            {
                var currentVersion = await _eventRepository.GetCountAsync(d => d.AggregateId == @event.AggregateRootId);
                var nextVersion = _versionService.GetNextVersion(@event.AggregateRootId, currentVersion, command?.ExpectedVersion);

                var eventDocument = _eventDocumentFactory.CreateEvent(@event, nextVersion);

                await _eventRepository.CreateDocumentAsync(eventDocument);
            }
        }
    }
}
