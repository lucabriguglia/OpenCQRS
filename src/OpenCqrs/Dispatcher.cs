using System.Threading.Tasks;
using OpenCqrs.Abstractions;
using OpenCqrs.Abstractions.Bus;
using OpenCqrs.Abstractions.Commands;
using OpenCqrs.Abstractions.Domain;
using OpenCqrs.Abstractions.Events;
using OpenCqrs.Abstractions.Queries;
using OpenCqrs.Bus;
using OpenCqrs.Commands;
using OpenCqrs.Domain;
using OpenCqrs.Events;
using OpenCqrs.Queries;

namespace OpenCqrs
{
    /// <inheritdoc />
    /// <summary>
    /// Dispatcher
    /// </summary>
    /// <seealso cref="T:OpenCqrs.IDispatcher" />
    public class Dispatcher : IDispatcher
    {
        private readonly ICommandSender _commandSender;
        private readonly IEventPublisher _eventPublisher;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IBusMessageDispatcher _busMessageDispatcher;

        public Dispatcher(ICommandSender commandSender, 
            IEventPublisher eventPublisher, 
            IQueryProcessor queryProcessor, 
            IBusMessageDispatcher busMessageDispatcher)
        {
            _commandSender = commandSender;
            _eventPublisher = eventPublisher;
            _queryProcessor = queryProcessor;
            _busMessageDispatcher = busMessageDispatcher;
        }

        /// <inheritdoc />
        public Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            return _commandSender.SendAsync(command);
        }

        /// <inheritdoc />
        public Task SendAsync<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand 
            where TAggregate : IAggregateRoot
        {
            return _commandSender.SendAsync<TCommand, TAggregate>(command);
        }

        /// <inheritdoc />
        public Task SendAndPublishAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            return _commandSender.SendAndPublishAsync(command);
        }

        /// <inheritdoc />
        public Task SendAndPublishAsync<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand 
            where TAggregate : IAggregateRoot
        {
            return _commandSender.SendAndPublishAsync<TCommand, TAggregate>(command);
        }

        /// <inheritdoc />
        public Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            return _eventPublisher.PublishAsync(@event);
        }

        /// <inheritdoc />
        public Task<TResult> GetResultAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            return _queryProcessor.ProcessAsync<TQuery, TResult>(query);
        }

        /// <inheritdoc />
        public Task DispatchBusMessageAsync<TMessage>(TMessage message) where TMessage : IBusMessage
        {
            return _busMessageDispatcher.DispatchAsync(message);
        }

        /// <inheritdoc />
        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            _commandSender.Send(command);
        }

        /// <inheritdoc />
        public void Send<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand 
            where TAggregate : IAggregateRoot
        {
            _commandSender.Send<TCommand, TAggregate>(command);
        }

        /// <inheritdoc />
        public void SendAndPublish<TCommand>(TCommand command) where TCommand : ICommand
        {
            _commandSender.SendAndPublish(command);
        }

        /// <inheritdoc />
        public void SendAndPublish<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand 
            where TAggregate : IAggregateRoot
        {
            _commandSender.SendAndPublish<TCommand, TAggregate>(command);
        }

        /// <inheritdoc />
        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            _eventPublisher.Publish(@event);
        }

        /// <inheritdoc />
        public TResult GetResult<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            return _queryProcessor.Process<TQuery, TResult>(query);
        }
    }
}