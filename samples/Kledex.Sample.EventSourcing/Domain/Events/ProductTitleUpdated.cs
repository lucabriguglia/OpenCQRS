using Kledex.Domain;

namespace Kledex.Samples.EventSourcing.Domain.Events
{
    public class ProductTitleUpdated : DomainEvent
    {
        public string Title { get; set; }
    }
}
