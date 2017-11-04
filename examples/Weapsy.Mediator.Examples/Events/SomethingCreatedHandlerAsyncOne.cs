using System.Threading.Tasks;
using Weapsy.Mediator.Events;

namespace Weapsy.Mediator.Examples.Events
{
    public class SomethingCreatedHandlerAsyncOne : IEventHandlerAsync<SomethingCreated>
    {
        public Task HandleAsync(SomethingCreated @event)
        {
            throw new System.NotImplementedException();
        }
    }
}
