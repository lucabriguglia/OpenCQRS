using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Kledex.Domain;

namespace Kledex.Models
{
    public class EventModel
    {
        public EventModel(IDomainEvent @event)
        {
            Id = @event.Id;
            AggregateRootId = @event.AggregateRootId;
            CommandId = @event.CommandId;
            UserId = @event.UserId;
            Source = @event.Source;
            TimeStamp = @event.TimeStamp;
            Type = @event.GetType().Name;
            
            var properties = @event.GetType()
                .GetProperties()
                .Where(x => x.Name != nameof(@event.Id) &&
                            x.Name != nameof(@event.AggregateRootId) &&
                            x.Name != nameof(@event.CommandId) &&
                            x.Name != nameof(@event.UserId) &&
                            x.Name != nameof(@event.Source) &&
                            x.Name != nameof(@event.TimeStamp));

            var data = new Dictionary<string, string>();

            foreach (var propertyInfo in properties)
            {
                data.Add(propertyInfo.Name, propertyInfo.GetValue(@event, null)?.ToString());
            }

            Data = new ReadOnlyDictionary<string, string>(data);
        }

        public Guid Id { get; }
        public Guid AggregateRootId { get; }
        public Guid CommandId { get; }
        public string UserId { get; }
        public string Source { get; }
        public DateTime TimeStamp { get; }
        public string Type { get; }
        public ReadOnlyDictionary<string, string> Data { get; }
    }
}
