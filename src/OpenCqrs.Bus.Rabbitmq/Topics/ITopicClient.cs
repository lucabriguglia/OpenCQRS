using System.Threading.Tasks;

namespace OpenCqrs.Bus.Rabbitmq.Topics
{
    public interface ITopicClient
    {
        Task SendAsync<TMessage>(TMessage message) where TMessage : IBusTopicMessage;
    }
}
