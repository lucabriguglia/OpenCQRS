using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Domain;
using Kledex.Store.Cosmos.Sql.Documents;
using Kledex.Store.Cosmos.Sql.Documents.Factories;
using Kledex.Store.Cosmos.Sql.Repositories;
using Newtonsoft.Json;

namespace Kledex.Store.Cosmos.Sql
{
    /// <inheritdoc />
    public class CommandStore : ICommandStore
    {
        private readonly IDocumentRepository<CommandDocument> _commandRepository;
        private readonly ICommandDocumentFactory _commandDocumentFactory;

        public CommandStore(IDocumentRepository<CommandDocument> commandRepository, ICommandDocumentFactory commandDocumentFactory)
        {
            _commandRepository = commandRepository;
            _commandDocumentFactory = commandDocumentFactory;
        }

        /// <inheritdoc />
        public Task SaveCommandAsync<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
            var commandDocument = _commandDocumentFactory.CreateCommand(command);
            return _commandRepository.CreateDocumentAsync(commandDocument);
        }

        /// <inheritdoc />
        public void SaveCommand<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
            var commandDocument = _commandDocumentFactory.CreateCommand(command);
            _commandRepository.CreateDocumentAsync(commandDocument).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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
