using System.Threading.Tasks;
using OpenCqrs.Abstractions.Bus;

namespace OpenCqrs.Bus
{
    public interface IBusMessageDispatcher
    {
        Task DispatchAsync<TMessage>(TMessage message) where TMessage : IBusMessage;
    }
}
