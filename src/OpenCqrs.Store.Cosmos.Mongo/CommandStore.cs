using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using OpenCqrs.Domain;
using OpenCqrs.Store.Cosmos.Mongo.Configuration;
using OpenCqrs.Store.Cosmos.Mongo.Documents;
using OpenCqrs.Store.Cosmos.Mongo.Documents.Factories;

namespace OpenCqrs.Store.Cosmos.Mongo
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
        public Task SaveCommandAsync<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
            var commandDocument = _commandDocumentFactory.CreateCommand(command);
            return _dbContext.Commands.InsertOneAsync(commandDocument);
        }

        /// <inheritdoc />
        public void SaveCommand<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
            var commandDocument = _commandDocumentFactory.CreateCommand(command);
            _dbContext.Commands.InsertOne(commandDocument);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DomainCommand>> GetCommandsAsync(Guid aggregateId)
        {
            var result = new List<DomainCommand>();

            var filter = Builders<CommandDocument>.Filter.Eq("aggregateId", aggregateId.ToString());
            var commands = await _dbContext.Commands.Find(filter).ToListAsync();

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

            var filter = Builders<CommandDocument>.Filter.Eq("aggregateId", aggregateId.ToString());
            var commands = _dbContext.Commands.Find(filter).ToList();

            foreach (var command in commands)
            {
                var domainCommand = JsonConvert.DeserializeObject(command.Data, Type.GetType(command.Type));
                result.Add((DomainCommand)domainCommand);
            }

            return result;
        }
    }
}
