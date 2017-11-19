using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Weapsy.Mediator.Domain;
using Weapsy.Mediator.EventStore.EF.Factories;

namespace Weapsy.Mediator.EventStore.EF
{
    public class EventStore : IEventStore
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IAggregateEntityFactory _aggregateEntityFactory;
        private readonly IEventEntityFactory _eventEntityFactory;

        public EventStore(IDbContextFactory dbContextFactory,
            IAggregateEntityFactory aggregateEntityFactory,
            IEventEntityFactory eventEntityFactory)
        {
            _dbContextFactory = dbContextFactory;
            _aggregateEntityFactory = aggregateEntityFactory;
            _eventEntityFactory = eventEntityFactory;            
        }

        public async Task SaveEventAsync<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var aggregateEntity = await dbContext.Aggregates.FirstOrDefaultAsync(x => x.Id == @event.AggregateId);               
                if (aggregateEntity == null)
                {
                    var newAggregateEntity = _aggregateEntityFactory.CreateAggregate<TAggregate>(@event);
                    dbContext.Aggregates.Add(newAggregateEntity);
                }

                var currentSequenceCount = await dbContext.Events.CountAsync(x => x.AggregateId == @event.AggregateId);
                var newEventEntity = _eventEntityFactory.CreateEvent(@event, currentSequenceCount + 1);
                dbContext.Events.Add(newEventEntity);

                await dbContext.SaveChangesAsync();
            }
        }

        public void SaveEvent<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var aggregateEntity = dbContext.Aggregates.FirstOrDefault(x => x.Id == @event.AggregateId);
                if (aggregateEntity == null)
                {
                    var newAggregateEntity = _aggregateEntityFactory.CreateAggregate<TAggregate>(@event);
                    dbContext.Aggregates.Add(newAggregateEntity);
                }

                var currentSequenceCount = dbContext.Events.Count(x => x.AggregateId == @event.AggregateId);
                var newEventEntity = _eventEntityFactory.CreateEvent(@event, currentSequenceCount + 1);
                dbContext.Events.Add(newEventEntity);

                dbContext.SaveChanges();
            }
        }

        public async Task<IEnumerable<DomainEvent>> GetEventsAsync(Guid aggregateId)
        {
            var result = new List<DomainEvent>();

            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var events = await dbContext.Events.Where(x => x.AggregateId == aggregateId).ToListAsync();
                foreach (var @event in events)
                {
                    var domainEvent = JsonConvert.DeserializeObject(@event.Body, Type.GetType(@event.Type));
                    result.Add((DomainEvent)domainEvent);
                }
            }

            return result;
        }

        public IEnumerable<DomainEvent> GetEvents(Guid aggregateId)
        {
            var result = new List<DomainEvent>();

            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var events = dbContext.Events.Where(x => x.AggregateId == aggregateId).ToList();
                foreach (var @event in events)
                {
                    var domainEvent = JsonConvert.DeserializeObject(@event.Body, Type.GetType(@event.Type));
                    result.Add((DomainEvent)domainEvent);
                }
            }

            return result;
        }
    }
}
