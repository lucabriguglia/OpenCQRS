using System.Threading.Tasks;

namespace Weapsy.Mediator.Events
{
    public interface IEventHandlerAsync<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
