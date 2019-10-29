using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Domain;
using Kledex.Store.Cosmos.Mongo.Configuration;
using Kledex.Store.Cosmos.Mongo.Documents;
using Kledex.Store.Cosmos.Mongo.Documents.Factories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Kledex.Store.Cosmos.Mongo
{
    /// <inheritdoc />
    public class CommandStore : ICommandStore
    {
        private readonly DomainDbContext _dbContext;
        private readonly ICommandDocumentFactory _commandDocumentFactory;

        public CommandStore(IOptions<DomainDbConfiguration> settings, ICommandDocumentFactory commandDocumentFactory)
        {
            _dbContext = new DomainDbContext(settings);
            _commandDocumentFactory = commandDocumentFactory;
        }

        /// <inheritdoc />
        public Task SaveCommandAsync(IDomainCommand command)
        {
            var commandDocument = _commandDocumentFactory.CreateCommand(command);
            return _dbContext.Commands.InsertOneAsync(commandDocument);
        }

        /// <inheritdoc />
        public void SaveCommand(IDomainCommand command)
        {
            var commandDocument = _commandDocumentFactory.CreateCommand(command);
            _dbContext.Commands.InsertOne(commandDocument);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<IDomainCommand>> GetCommandsAsync(Guid aggregateId)
        {
            var result = new List<IDomainCommand>();

            var filter = Builders<CommandDocument>.Filter.Eq("aggregateId", aggregateId.ToString());
            var commands = await _dbContext.Commands.Find(filter).ToListAsync();

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

            var filter = Builders<CommandDocument>.Filter.Eq("aggregateId", aggregateId.ToString());
            var commands = _dbContext.Commands.Find(filter).ToList();

            foreach (var command in commands)
            {
                var domainCommand = JsonConvert.DeserializeObject(command.Data, Type.GetType(command.Type));
                result.Add((IDomainCommand)domainCommand);
            }

            return result;
        }
    }
}
