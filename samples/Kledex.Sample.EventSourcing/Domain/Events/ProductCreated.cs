using Kledex.Domain;

namespace Kledex.Samples.EventSourcing.Domain.Events
{
    public class ProductCreated : DomainEvent
    {
        public string Title { get; set; }
        public ProductStatus Status { get; set; }
    }
}
