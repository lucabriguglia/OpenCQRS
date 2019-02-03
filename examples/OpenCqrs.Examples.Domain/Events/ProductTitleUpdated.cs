using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Examples.Domain.Events
{
    public class ProductTitleUpdated : DomainEvent
    {
        public string Title { get; set; }
    }
}
