using System;
using Weapsy.Mediator.Domain;
using Weapsy.Mediator.Examples.Domain.Events;

namespace Weapsy.Mediator.Examples.Domain
{
    public class Product : AggregateRoot
    {
        public string Title { get; private set; }

        public Product()
        {            
        }

        public Product(Guid id, string title) : base(id)
        {
            if (string.IsNullOrEmpty(title))
                throw new ApplicationException("Product title is required.");

            Title = title;

            AddEvent(new ProductCreated
            {
                AggregateId = Id,
                Title = Title
            });
        }

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ApplicationException("Product title is required.");

            Title = title;

            AddEvent(new ProductTitleUpdated
            {
                AggregateId = Id,
                Title = Title
            });
        }

        public void Apply(ProductCreated @event)
        {
            Id = @event.AggregateId;
            Title = @event.Title;
        }

        public void Apply(ProductTitleUpdated @event)
        {
            Title = @event.Title;
        }
    }
}
