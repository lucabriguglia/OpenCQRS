using Weapsy.Mediator.Events;

namespace Weapsy.Mediator.Examples.Events
{
    public class SomethingCreatedHandlerTwo : IEventHandler<SomethingCreated>
    {
        public void Handle(SomethingCreated @event)
        {
            throw new System.NotImplementedException();
        }
    }
}
