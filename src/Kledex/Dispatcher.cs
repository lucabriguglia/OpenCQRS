using System.Threading.Tasks;
using Kledex.Bus;
using Kledex.Commands;
using Kledex.Domain;
using Kledex.Events;
using Kledex.Queries;

namespace Kledex
{
    /// <inheritdoc />
    /// <summary>
    /// Dispatcher
    /// </summary>
    /// <seealso cref="T:Kledex.IDispatcher" />
    public class Dispatcher : IDispatcher
    {
        private readonly ICommandSender _commandSender;
        private readonly IEventPublisher _eventPublisher;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IBusMessageDispatcher _busMessageDispatcher;

        public Dispatcher(ICommandSender domainCommandSender,
            IEventPublisher eventPublisher,
            IQueryProcessor queryProcessor,
            IBusMessageDispatcher busMessageDispatcher)
        {
            _commandSender = domainCommandSender;
            _eventPublisher = eventPublisher;
            _queryProcessor = queryProcessor;
            _busMessageDispatcher = busMessageDispatcher;
        }

        /// <inheritdoc />
        public Task SendAsync(ICommand command)
        {
            return _commandSender.SendAsync(command);
        }

        /// <inheritdoc />
        public Task SendAsync(ICommandSequence sequenceCommand)
        {
            return _commandSender.SendAsync(sequenceCommand);
        }

        /// <inheritdoc />
        public Task<TResult> SendAsync<TResult>(ICommand command)
        {
            return _commandSender.SendAsync<TResult>(command);
        }

        /// <inheritdoc />
        public Task<TResult> SendAsync<TResult>(ICommandSequence sequenceCommand)
        {
            return _commandSender.SendAsync<TResult>(sequenceCommand);
        }

        /// <inheritdoc />
        public Task PublishAsync<TEvent>(TEvent @event) 
            where TEvent : IEvent
        {
            return _eventPublisher.PublishAsync(@event);
        }

        /// <inheritdoc />
        public Task<TResult> GetResultAsync<TResult>(IQuery<TResult> query)
        {
            return _queryProcessor.ProcessAsync(query);
        }

        /// <inheritdoc />
        public Task DispatchBusMessageAsync<TMessage>(TMessage message) 
            where TMessage : IBusMessage
        {
            return _busMessageDispatcher.DispatchAsync(message);
        }

        /// <inheritdoc />
        public void Send(ICommand command)
        {
            _commandSender.Send(command);
        }

        /// <inheritdoc />
        public void Send(ICommandSequence sequenceCommand)
        {
            _commandSender.Send(sequenceCommand);
        }

        /// <inheritdoc />
        public TResult Send<TResult>(ICommand command)
        {
            return _commandSender.Send<TResult>(command);
        }

        /// <inheritdoc />
        public TResult Send<TResult>(ICommandSequence sequenceCommand)
        {
            return _commandSender.Send<TResult>(sequenceCommand);
        }

        /// <inheritdoc />
        public void Publish<TEvent>(TEvent @event) 
            where TEvent : IEvent
        {
            _eventPublisher.Publish(@event);
        }

        /// <inheritdoc />
        public TResult GetResult<TResult>(IQuery<TResult> query)
        {
            return _queryProcessor.Process(query);
        }
    }
}