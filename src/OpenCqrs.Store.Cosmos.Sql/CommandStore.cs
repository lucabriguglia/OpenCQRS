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
    internal class CommandStore : ICommandStore
    {
        private readonly IDocumentRepository<AggregateDocument> _aggregateRepository;
        private readonly IDocumentRepository<CommandDocument> _commandRepository;
        private readonly IAggregateDocumentFactory _aggregateDocumentFactory;
        private readonly ICommandDocumentFactory _commandDocumentFactory;

        public CommandStore(IDocumentRepository<AggregateDocument> aggregateRepository, 
            IDocumentRepository<CommandDocument> commandRepository,
            IAggregateDocumentFactory aggregateDocumentFactory,
            ICommandDocumentFactory commandDocumentFactory)
        {
            _aggregateRepository = aggregateRepository;
            _commandRepository = commandRepository;
            _aggregateDocumentFactory = aggregateDocumentFactory;
            _commandDocumentFactory = commandDocumentFactory;
        }

        public async Task SaveCommandAsync<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
            var aggregateDocument = await _aggregateRepository.GetDocumentAsync(command.AggregateRootId.ToString());
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(command.AggregateRootId);
                await _aggregateRepository.CreateDocumentAsync(newAggregateDocument);
            }

            var commandDocument = _commandDocumentFactory.CreateCommand(command);
            await _commandRepository.CreateDocumentAsync(commandDocument);
        }

        public void SaveCommand<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
            var aggregateDocument = _aggregateRepository.GetDocumentAsync(command.AggregateRootId.ToString()).GetAwaiter().GetResult();
            if (aggregateDocument == null)
            {
                var newAggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(command.AggregateRootId);
                _aggregateRepository.CreateDocumentAsync(newAggregateDocument).GetAwaiter().GetResult();
            }

            var commandDocument = _commandDocumentFactory.CreateCommand(command);
            _commandRepository.CreateDocumentAsync(commandDocument).GetAwaiter().GetResult();
        }

        public async Task<IEnumerable<DomainCommand>> GetCommandsAsync(Guid aggregateId)
        {
            var result = new List<DomainCommand>();

            var commands = await _commandRepository.GetDocumentsAsync(d => d.AggregateId == aggregateId);

            foreach (var command in commands)
            {
                var domainCommand = JsonConvert.DeserializeObject(command.Data, Type.GetType(command.Type));
                result.Add((DomainCommand)domainCommand);
            }
 
            return result;
        }

        public IEnumerable<DomainCommand> GetCommands(Guid aggregateId)
        {
            var result = new List<DomainCommand>();

            var commands = _commandRepository.GetDocumentsAsync(d => d.AggregateId == aggregateId).GetAwaiter().GetResult();

            foreach (var command in commands)
            {
                var domainCommand = JsonConvert.DeserializeObject(command.Data, Type.GetType(command.Type));
                result.Add((DomainCommand)domainCommand);
            }

            return result;
        }
    }
}
