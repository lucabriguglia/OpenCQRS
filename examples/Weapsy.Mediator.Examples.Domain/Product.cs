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

            AddEvent(new ProductCreated
            {
                AggregateRootId = Id,
                Title = title
            });
        }

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ApplicationException("Product title is required.");

            AddEvent(new ProductTitleUpdated
            {
                AggregateRootId = Id,
                Title = title
            });
        }

        public void Apply(ProductCreated @event)
        {
            Id = @event.AggregateRootId;
            Title = @event.Title;
        }

        public void Apply(ProductTitleUpdated @event)
        {
            Title = @event.Title;
        }
    }
}
