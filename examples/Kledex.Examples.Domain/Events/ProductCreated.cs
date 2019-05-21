using OpenCqrs.Domain;

namespace OpenCqrs.Examples.Domain.Events
{
    public class ProductCreated : DomainEvent
    {
        public string Title { get; set; }
        public ProductStatus Status { get; set; }
    }
}
