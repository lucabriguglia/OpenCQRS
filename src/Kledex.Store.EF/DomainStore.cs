using Kledex.Domain;
using Kledex.Store.EF.Entities.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kledex.Store.EF
{
    public class DomainStore : IDomainStore
    {
        private readonly IDomainDbContextFactory _dbContextFactory;
        private readonly IAggregateEntityFactory _aggregateEntityFactory;
        private readonly ICommandEntityFactory _commandEntityFactory;
        private readonly IEventEntityFactory _eventEntityFactory;
        private readonly IVersionService _versionService;

        public DomainStore(IDomainDbContextFactory dbContextFactory, 
            IAggregateEntityFactory aggregateEntityFactory,
            ICommandEntityFactory commandEntityFactory,
            IEventEntityFactory eventEntityFactory,
            IVersionService versionService)
        {
            _dbContextFactory = dbContextFactory;
            _aggregateEntityFactory = aggregateEntityFactory;
            _commandEntityFactory = commandEntityFactory;
            _eventEntityFactory = eventEntityFactory;
            _versionService = versionService;
        }

        public IEnumerable<DomainEvent> GetEvents(Guid aggregateId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId)
        {
            throw new NotImplementedException();
        }

        public void Save<TAggregate>(SaveDomainData request) where TAggregate : IAggregateRoot
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync<TAggregate>(SaveDomainData request) where TAggregate : IAggregateRoot
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                if (request.Properties.ContainsKey(Consts.DbContextTransactionKey))
                {
                    var dbContextTransaction = request.Properties[Consts.DbContextTransactionKey] as IDbContextTransaction;
                    dbContext.Database.UseTransaction(dbContextTransaction.GetDbTransaction());
                }

                var aggregateEntity = await dbContext.Aggregates.FirstOrDefaultAsync(x => x.Id == request.Command.AggregateRootId);
                if (aggregateEntity == null)
                {
                    var newAggregateEntity = _aggregateEntityFactory.CreateAggregate<TAggregate>(request.Command.AggregateRootId);
                    await dbContext.Aggregates.AddAsync(newAggregateEntity);                    
                }

                var newCommandEntity = _commandEntityFactory.CreateCommand(request.Command);
                await dbContext.Commands.AddAsync(newCommandEntity);

                foreach (var @event in request.Events)
                {
                    var currentVersion = await dbContext.Events.CountAsync(x => x.AggregateId == @event.AggregateRootId);
                    var nextVersion = _versionService.GetNextVersion(@event.AggregateRootId, currentVersion, request.Command.ExpectedVersion);
                    var newEventEntity = _eventEntityFactory.CreateEvent(@event, nextVersion);
                    await dbContext.Events.AddAsync(newEventEntity);
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
