using Kledex.Domain;
using Kledex.Sample.EventSourcing.Domain.Events;

namespace Kledex.Sample.EventSourcing.Domain
{
    public class Product : AggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductStatus Status { get; set; }

        public Product()
        {
        }

        public Product(string name, string description, decimal price)
        {
            AddAndApplyEvent(new ProductCreated
            {
                AggregateRootId = Id,
                Name = name,
                Description = description,
                Price = price,
                Status = ProductStatus.Draft
            });
        }

        public void Update(string name, string description, decimal price)
        {
            AddAndApplyEvent(new ProductUpdated
            {
                Name = name,
                Description = description,
                Price = price
            });
        }

        public void Publish()
        {
            AddAndApplyEvent(new ProductPublished
            {
                AggregateRootId = Id
            });
        }

        public void Withdraw()
        {
            AddAndApplyEvent(new ProductWithdrew
            {
                AggregateRootId = Id
            });
        }

        public void Delete()
        {
            AddAndApplyEvent(new ProductDeleted
            {
                AggregateRootId = Id
            });
        }

        private void Apply(ProductCreated @event)
        {
            Id = @event.AggregateRootId;
            Name = @event.Name;
            Description = @event.Description;
            Price = @event.Price;
            Status = @event.Status;
        }

        private void Apply(ProductDeleted @event)
        {
            Status = ProductStatus.Deleted;
        }

        private void Apply(ProductPublished @event)
        {
            Status = ProductStatus.Published;
        }

        private void Apply(ProductUpdated @event)
        {
            Name = @event.Name;
            Description = @event.Description;
            Price = @event.Price;
        }

        private void Apply(ProductWithdrew @event)
        {
            Status = ProductStatus.Draft;
        }
    }
}
