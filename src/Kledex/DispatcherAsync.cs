using System;
using System.Threading.Tasks;
using Kledex.Bus;
using Kledex.Commands;
using Kledex.Domain;
using Kledex.Events;

namespace Kledex
{
    /// <inheritdoc />
    /// <summary>
    /// Dispatcher
    /// </summary>
    /// <seealso cref="T:Kledex.IDispatcher" />
    public partial class Dispatcher : IDispatcher
    {
        private readonly ICommandSender _commandSender;
        private readonly IEventPublisher _eventPublisher;
        private readonly IBusMessageDispatcher _busMessageDispatcher;

        public Dispatcher(ICommandSender domainCommandSender,
            IEventPublisher eventPublisher,
            IBusMessageDispatcher busMessageDispatcher)
        {
            _commandSender = domainCommandSender;
            _eventPublisher = eventPublisher;
            _busMessageDispatcher = busMessageDispatcher;
        }

        /// <inheritdoc />
        public Task SendAsync<TCommand>(TCommand command) 
            where TCommand : ICommand
        {
            return _commandSender.SendAsync(command);
        }

        /// <inheritdoc />
        public Task SendAsync<TCommand>(TCommand command, Func<Task<CommandResponse>> commandHandler) 
            where TCommand : ICommand
        {
            return _commandSender.SendAsync(command, commandHandler);
        }

        /// <inheritdoc />
        public Task SendAsync(ICommandSequence commandSequence)
        {
            return _commandSender.SendAsync(commandSequence);
        }

        /// <inheritdoc />
        public Task<TResult> SendAsync<TResult>(ICommand command)
        {
            return _commandSender.SendAsync<TResult>(command);
        }

        /// <inheritdoc />
        public Task<TResult> SendAsync<TResult>(ICommand command, Func<Task<CommandResponse>> commandHandler)
        {
            return _commandSender.SendAsync<TResult>(command, commandHandler);
        }

        /// <inheritdoc />
        public Task<TResult> SendAsync<TResult>(ICommandSequence commandSequence)
        {
            return _commandSender.SendAsync<TResult>(commandSequence);
        }

        /// <inheritdoc />
        public Task PublishAsync<TEvent>(TEvent @event) 
            where TEvent : IEvent
        {
            return _eventPublisher.PublishAsync(@event);
        }

        /// <inheritdoc />
        public Task DispatchBusMessageAsync<TMessage>(TMessage message) 
            where TMessage : IBusMessage
        {
            return _busMessageDispatcher.DispatchAsync(message);
        }
    }
}