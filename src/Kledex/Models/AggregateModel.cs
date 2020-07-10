using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Kledex.Domain;

namespace Kledex.Models
{
    public class AggregateModel
    {
        public AggregateModel(IEnumerable<IDomainEvent> events)
        {
            var domainEvents = events as IDomainEvent[] ?? events.OrderByDescending(x => x.TimeStamp).ToArray();

            foreach (var @event in domainEvents)
            {
                _events.Add(new EventModel(@event));
                Version++;
            }
        }

        public int Version { get; }

        public ReadOnlyCollection<EventModel> Events => _events.AsReadOnly();
        private readonly List<EventModel> _events = new List<EventModel>();
    }
}
