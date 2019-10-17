using Kledex.Domain;

namespace Kledex.Sample.EventSourcing.Domain.Events
{
    public class ProductUpdated : DomainEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
