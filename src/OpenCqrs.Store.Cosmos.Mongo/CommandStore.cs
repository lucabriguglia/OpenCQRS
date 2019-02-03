using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using OpenCqrs.Abstractions.Domain;
using OpenCqrs.Domain;
using OpenCqrs.Store.Cosmos.Mongo.Configuration;
using OpenCqrs.Store.Cosmos.Mongo.Documents;
using OpenCqrs.Store.Cosmos.Mongo.Documents.Factories;

namespace OpenCqrs.Store.Cosmos.Mongo
{
    public class CommandStore : ICommandStore
    {
        private readonly DomainDbContext _dbContext;
        private readonly IAggregateDocumentFactory _aggregateDocumentFactory;
        private readonly ICommandDocumentFactory _commandDocumentFactory;

        public CommandStore(IOptions<DomainDbConfiguration> settings, 
            IAggregateDocumentFactory aggregateDocumentFactory, 
            ICommandDocumentFactory commandDocumentFactory)
        {
            _dbContext = new DomainDbContext(settings);
            _aggregateDocumentFactory = aggregateDocumentFactory;
            _commandDocumentFactory = commandDocumentFactory;
        }

        public async Task SaveCommandAsync<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
            var aggregateFilter = Builders<AggregateDocument>.Filter.Eq("_id", command.AggregateRootId.ToString());
            var aggregate = await _dbContext.Aggregates.Find(aggregateFilter).FirstOrDefaultAsync();
            if (aggregate == null)
            {
                var aggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(command.AggregateRootId);
                await _dbContext.Aggregates.InsertOneAsync(aggregateDocument);
            }

            var commandDocument = _commandDocumentFactory.CreateCommand(command);
            await _dbContext.Commands.InsertOneAsync(commandDocument);
        }

        public void SaveCommand<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
            var aggregateFilter = Builders<AggregateDocument>.Filter.Eq("_id", command.AggregateRootId.ToString());
            var aggregate = _dbContext.Aggregates.Find(aggregateFilter).FirstOrDefault();
            if (aggregate == null)
            {
                var aggregateDocument = _aggregateDocumentFactory.CreateAggregate<TAggregate>(command.AggregateRootId);
                _dbContext.Aggregates.InsertOne(aggregateDocument);
            }

            var commandDocument = _commandDocumentFactory.CreateCommand(command);
            _dbContext.Commands.InsertOne(commandDocument);
        }

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
