using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.Tests.Fakes
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