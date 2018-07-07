using OpenCqrs.Domain;

namespace OpenCqrs.Tests.Fakes
{
    public class Aggregate : AggregateRootWithEvents
    {
        public Aggregate()
        {
            AddAndApplyEvent(new AggregateCreated());
        }

        private void Apply(AggregateCreated @event)
        {           
        }
    }
}