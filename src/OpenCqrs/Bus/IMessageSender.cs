using System.Threading.Tasks;

namespace OpenCqrs.Bus
{
    public interface IMessageSender
    {
        Task SendAsync<TMessage>(TMessage message) where TMessage : IBusMessage;
    }
}
