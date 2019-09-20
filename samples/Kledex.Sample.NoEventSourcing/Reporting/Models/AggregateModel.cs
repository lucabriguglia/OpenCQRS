using Kledex.Domain;
using ReflectionMagic;
using System.Collections.Generic;
using System.Linq;

namespace Kledex.Sample.NoEventSourcing.Reporting.Models
{
    public class AggregateModel
    {
        public IList<EventModel> Events { get; set; } = new List<EventModel>();
        public int Version { get; set; }

        public void AddEvents(IEnumerable<IDomainEvent> events)
        {
            var domainEvents = events as IDomainEvent[] ?? events.OrderByDescending(x => x.TimeStamp).ToArray();

            foreach (var @event in domainEvents)
            {
                this.AsDynamic().Add(@event);
                Version++;
            }
        }
    }
}
