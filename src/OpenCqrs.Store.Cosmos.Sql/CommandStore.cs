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
    internal class CommandStore : ICommandStore
    {
        private readonly IDocumentRepository<CommandDocument> _commandRepository;
        private readonly ICommandDocumentFactory _commandDocumentFactory;

        public CommandStore(IDocumentRepository<CommandDocument> commandRepository, ICommandDocumentFactory commandDocumentFactory)
        {
            _commandRepository = commandRepository;
            _commandDocumentFactory = commandDocumentFactory;
        }

        public Task SaveCommandAsync<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
            var commandDocument = _commandDocumentFactory.CreateCommand(command);
            return _commandRepository.CreateDocumentAsync(commandDocument);
        }

        public void SaveCommand<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
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
