using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.Examples.Domain.Events
{
    public class ProductCreated : DomainEvent
    {
        public string Title { get; set; }
    }
}
