using System.Threading.Tasks;
using OpenCqrs.Abstractions.Bus;

namespace OpenCqrs.Bus.Topics
{
    public interface ITopicClient
    {
        Task SendAsync<TMessage>(TMessage message) where TMessage : IBusTopicMessage;
    }
}
