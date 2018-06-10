using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Weapsy.Cqrs.Domain;
using Weapsy.Cqrs.EF.Entities.Factories;

namespace Weapsy.Cqrs.EF.Stores
{
    public class CommandStore : ICommandStore
    {
        private readonly IDomainDbContextFactory _dbContextFactory;
        private readonly IAggregateEntityFactory _aggregateEntityFactory;
        private readonly ICommandEntityFactory _commandEntityFactory;

        public CommandStore(IDomainDbContextFactory dbContextFactory,
            IAggregateEntityFactory aggregateEntityFactory,
            ICommandEntityFactory commandEntityFactory)
        {
            _dbContextFactory = dbContextFactory;
            _aggregateEntityFactory = aggregateEntityFactory;
            _commandEntityFactory = commandEntityFactory;            
        }

        /// <inheritdoc />
        public async Task SaveCommandAsync<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var aggregateEntity = await dbContext.Aggregates.FirstOrDefaultAsync(x => x.Id == command.AggregateRootId);               
                if (aggregateEntity == null)
                {
                    var newAggregateEntity = _aggregateEntityFactory.CreateAggregate<TAggregate>(command.AggregateRootId);
                    dbContext.Aggregates.Add(newAggregateEntity);
                }

                var currentSequenceCount = await dbContext.Commands.CountAsync(x => x.AggregateId == command.AggregateRootId);
                var newCommandEntity = _commandEntityFactory.CreateCommand(command);
                dbContext.Commands.Add(newCommandEntity);

                await dbContext.SaveChangesAsync();
            }
        }

        /// <inheritdoc />
        public void SaveCommand<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var aggregateEntity = dbContext.Aggregates.FirstOrDefault(x => x.Id == command.AggregateRootId);
                if (aggregateEntity == null)
                {
                    var newAggregateEntity = _aggregateEntityFactory.CreateAggregate<TAggregate>(command.AggregateRootId);
                    dbContext.Aggregates.Add(newAggregateEntity);
                }

                var currentSequenceCount = dbContext.Commands.Count(x => x.AggregateId == command.AggregateRootId);
                var newCommandEntity = _commandEntityFactory.CreateCommand(command);
                dbContext.Commands.Add(newCommandEntity);

                dbContext.SaveChanges();
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DomainCommand>> GetCommandsAsync(Guid aggregateId)
        {
            var result = new List<DomainCommand>();

            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var commands = await dbContext.Commands.Where(x => x.AggregateId == aggregateId).ToListAsync();
                foreach (var command in commands)
                {
                    var domainCommand = JsonConvert.DeserializeObject(command.Data, Type.GetType(command.Type));
                    result.Add((DomainCommand)domainCommand);
                }
            }

            return result;
        }

        /// <inheritdoc />
        public IEnumerable<DomainCommand> GetCommands(Guid aggregateId)
        {
            var result = new List<DomainCommand>();

            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var commands = dbContext.Commands.Where(x => x.AggregateId == aggregateId).ToList();
                foreach (var command in commands)
                {
                    var domainCommand = JsonConvert.DeserializeObject(command.Data, Type.GetType(command.Type));
                    result.Add((DomainCommand)domainCommand);
                }
            }

            return result;
        }
    }
}
