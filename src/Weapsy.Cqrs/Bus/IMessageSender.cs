using System.Threading.Tasks;

namespace Weapsy.Cqrs.Bus
{
    public interface IMessageSender
    {
        Task SendAsync<TMessage>(TMessage message) where TMessage : IBusMessage;
    }
}
