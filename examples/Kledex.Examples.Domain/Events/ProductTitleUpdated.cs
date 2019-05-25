using Kledex.Domain;

namespace Kledex.Examples.Domain.Events
{
    public class ProductTitleUpdated : DomainEvent
    {
        public string Title { get; set; }
    }
}
