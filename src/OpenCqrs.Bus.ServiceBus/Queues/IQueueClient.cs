using System.Threading.Tasks;

namespace OpenCqrs.Bus.ServiceBus.Queues
{
    public interface IQueueClient
    {
        Task SendAsync<TMessage>(TMessage message) where TMessage : IBusQueueMessage;
    }
}
