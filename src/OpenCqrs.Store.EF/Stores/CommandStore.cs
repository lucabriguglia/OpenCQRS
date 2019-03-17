using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenCqrs.Domain;
using OpenCqrs.Store.EF.Entities.Factories;

namespace OpenCqrs.Store.EF.Stores
{
    /// <inheritdoc />
    public class CommandStore : ICommandStore
    {
        private readonly IDomainDbContextFactory _dbContextFactory;
        private readonly ICommandEntityFactory _commandEntityFactory;

        public CommandStore(IDomainDbContextFactory dbContextFactory, ICommandEntityFactory commandEntityFactory)
        {
            _dbContextFactory = dbContextFactory;
            _commandEntityFactory = commandEntityFactory;            
        }

        /// <inheritdoc />
        public async Task SaveCommandAsync<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var newCommandEntity = _commandEntityFactory.CreateCommand(command);
                await dbContext.Commands.AddAsync(newCommandEntity);
                await dbContext.SaveChangesAsync();
            }
        }

        /// <inheritdoc />
        public void SaveCommand<TAggregate>(IDomainCommand command) where TAggregate : IAggregateRoot
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
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
