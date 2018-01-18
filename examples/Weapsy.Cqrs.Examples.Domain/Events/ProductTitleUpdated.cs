using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Examples.Domain.Events
{
    public class ProductTitleUpdated : DomainEvent
    {
        public string Title { get; set; }
    }
}
