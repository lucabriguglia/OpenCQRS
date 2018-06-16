using OpenCqrs.Domain;

namespace OpenCqrs.Tests.Fakes
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