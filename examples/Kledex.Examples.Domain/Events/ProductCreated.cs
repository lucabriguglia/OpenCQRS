using Kledex.Domain;

namespace Kledex.Examples.Domain.Events
{
    public class ProductCreated : DomainEvent
    {
        public string Title { get; set; }
        public ProductStatus Status { get; set; }
    }
}
