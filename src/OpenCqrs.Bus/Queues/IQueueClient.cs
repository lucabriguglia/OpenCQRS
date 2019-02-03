using System.Threading.Tasks;
using OpenCqrs.Abstractions.Bus;

namespace OpenCqrs.Bus.Queues
{
    public interface IQueueClient
    {
        Task SendAsync<TMessage>(TMessage message) where TMessage : IBusQueueMessage;
    }
}
