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
        public Task SaveCommandAsync(IDomainCommand command)
        {
            var commandDocument = _commandDocumentFactory.CreateCommand(command);
            return _commandRepository.CreateDocumentAsync(commandDocument);
        }

        /// <inheritdoc />
        public void SaveCommand(IDomainCommand command)
        {
            var commandDocument = _commandDocumentFactory.CreateCommand(command);
            _commandRepository.CreateDocumentAsync(commandDocument).GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IDomainCommand>> GetCommandsAsync(Guid aggregateId)
        {
            var result = new List<IDomainCommand>();

            var commands = await _commandRepository.GetDocumentsAsync(d => d.AggregateId == aggregateId);

            foreach (var command in commands)
            {
                var domainCommand = JsonConvert.DeserializeObject(command.Data, Type.GetType(command.Type));
                result.Add((IDomainCommand)domainCommand);
            }
 
            return result;
        }

        /// <inheritdoc />
        public IEnumerable<IDomainCommand> GetCommands(Guid aggregateId)
        {
            var result = new List<IDomainCommand>();

            var commands = _commandRepository.GetDocumentsAsync(d => d.AggregateId == aggregateId).GetAwaiter().GetResult();

            foreach (var command in commands)
            {
                var domainCommand = JsonConvert.DeserializeObject(command.Data, Type.GetType(command.Type));
                result.Add((IDomainCommand)domainCommand);
            }

            return result;
        }
    }
}
