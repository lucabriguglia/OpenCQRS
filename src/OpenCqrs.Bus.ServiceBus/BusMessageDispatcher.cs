using System;
using System.Threading.Tasks;
using OpenCqrs.Bus.ServiceBus.Queues;

namespace OpenCqrs.Bus.ServiceBus
{
    internal class BusMessageDispatcher : IBusMessageDispatcher
    {
        private readonly IQueueClient _queueClient;

        public BusMessageDispatcher(IQueueClient queueClient)
        {
            _queueClient = queueClient;          
        }

        public Task DispatchAsync<TMessage>(TMessage message) where TMessage : IBusMessage
        {
            if (message is IBusQueueMessage queueMessage)
                return _queueClient.SendAsync(queueMessage);

            throw new NotSupportedException("The message must implement the IBusQueueMessage interface");
        }
    }
}
