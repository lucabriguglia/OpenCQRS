using System.Threading.Tasks;

namespace Kledex.Bus
{
    public interface IBusMessageDispatcher
    {
        Task DispatchAsync<TMessage>(TMessage message) where TMessage : IBusMessage;
    }
}
