using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.Examples.Domain.Events
{
    public class ProductTitleUpdated : DomainEvent
    {
        public string Title { get; set; }
    }
}
