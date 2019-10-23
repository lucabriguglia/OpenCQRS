using Kledex.Domain;

namespace Kledex.Sample.EventSourcing.Domain.Events
{
    public class ProductCreated : DomainEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductStatus Status { get; set; }
    }
}
