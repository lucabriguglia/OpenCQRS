using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Tests.Fakes
{
    public class Aggregate : AggregateRoot
    {
        public Aggregate()
        {
            AddEvent(new AggregateCreated());
        }

        private void Apply(AggregateCreated @event)
        {           
        }
    }
}