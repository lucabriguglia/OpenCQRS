using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Examples.Domain.Events
{
    public class ProductCreated : DomainEvent
    {
        public string Title { get; set; }
    }
}
