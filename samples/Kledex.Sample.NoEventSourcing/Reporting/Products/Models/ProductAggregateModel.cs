using System.Collections.Generic;
using Kledex.Sample.NoEventSourcing.Domain.Events;
using Kledex.Sample.NoEventSourcing.Reporting.Models;

namespace Kledex.Sample.NoEventSourcing.Reporting.Products.Models
{
    public class ProductAggregateModel : AggregateModel
    {
        private void Add(ProductCreated @event)
        {
            Events.Add(new EventModel
            {
                Type = "Created",
                TimeStamp = @event.TimeStamp,
                Data = new Dictionary<string, string>
                {
                    { "Name", @event.Name },
                    { "Description", @event.Description },
                    { "Price", @event.Price.ToString() }
                }
            });
        }

        private void Add(ProductUpdated @event)
        {
            Events.Add(new EventModel
            {
                Type = "Updated",
                TimeStamp = @event.TimeStamp,
                Data = new Dictionary<string, string>
                {
                    { "Name", @event.Name },
                    { "Description", @event.Description },
                    { "Price", @event.Price.ToString() }
                }
            });
        }

        private void Add(ProductPublished @event)
        {
            Events.Add(new EventModel
            {
                Type = "Published",
                TimeStamp = @event.TimeStamp
            });
        }

        private void Add(ProductWithdrew @event)
        {
            Events.Add(new EventModel
            {
                Type = "Withdrew",
                TimeStamp = @event.TimeStamp
            });
        }

        private void Add(ProductDeleted @event)
        {
            Events.Add(new EventModel
            {
                Type = "Deleted",
                TimeStamp = @event.TimeStamp
            });
        }
    }
}
