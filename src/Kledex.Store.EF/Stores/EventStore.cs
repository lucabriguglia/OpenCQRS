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
    public class EventStore : IEventStore
    {
        private readonly IDomainDbContextFactory _dbContextFactory;
        private readonly IEventEntityFactory _eventEntityFactory;
        private readonly IVersionService _versionService;

        public EventStore(IDomainDbContextFactory dbContextFactory,
            IEventEntityFactory eventEntityFactory, 
            IVersionService versionService)
        {
            _dbContextFactory = dbContextFactory;
            _eventEntityFactory = eventEntityFactory;
            _versionService = versionService;
        }

        /// <inheritdoc />
        public async Task SaveEventAsync<TAggregate>(IDomainEvent @event, int? expectedVersion) where TAggregate : IAggregateRoot
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var currentVersion = await dbContext.Events.CountAsync(x => x.AggregateId == @event.AggregateRootId);
                var nextVersion = _versionService.GetNextVersion(@event.AggregateRootId, currentVersion, expectedVersion);
                var newEventEntity = _eventEntityFactory.CreateEvent(@event, nextVersion);

                await dbContext.Events.AddAsync(newEventEntity);

                await dbContext.SaveChangesAsync();
            }
        }

        /// <inheritdoc />
        public void SaveEvent<TAggregate>(IDomainEvent @event, int? expectedVersion) where TAggregate : IAggregateRoot
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var currentVersion = dbContext.Events.Count(x => x.AggregateId == @event.AggregateRootId);
                var nextVersion = _versionService.GetNextVersion(@event.AggregateRootId, currentVersion, expectedVersion);
                var newEventEntity = _eventEntityFactory.CreateEvent(@event, nextVersion);

                dbContext.Events.Add(newEventEntity);

                dbContext.SaveChanges();
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId)
        {
            var result = new List<DomainEvent>();

            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var events = await dbContext.Events
                    .Where(x => x.AggregateId == aggregateId)
                    .OrderBy(x => x.Sequence)
                    .ToListAsync();

                foreach (var @event in events)
                {
                    var domainEvent = JsonConvert.DeserializeObject(@event.Data, Type.GetType(@event.Type));
                    result.Add((DomainEvent)domainEvent);
                }
            }

            return result;
        }

        /// <inheritdoc />
        public IEnumerable<DomainEvent> GetEvents(Guid aggregateId)
        {
            var result = new List<DomainEvent>();

            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var events = dbContext.Events
                    .Where(x => x.AggregateId == aggregateId)
                    .OrderBy(x => x.Sequence)
                    .ToList();

                foreach (var @event in events)
                {
                    var domainEvent = JsonConvert.DeserializeObject(@event.Data, Type.GetType(@event.Type));
                    result.Add((DomainEvent)domainEvent);
                }
            }

            return result;
        }
    }
}
