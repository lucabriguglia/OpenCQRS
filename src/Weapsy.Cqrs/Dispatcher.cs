using System.Threading.Tasks;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Cqrs.Events;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Cqrs
{
    /// <inheritdoc />
    /// <summary>
    /// Dispatcher
    /// </summary>
    /// <seealso cref="T:Weapsy.Cqrs.IDispatcher" />
    public class Dispatcher : IDispatcher
    {
        private readonly ICommandSenderAsync _commandSenderAsync;
        private readonly ICommandSender _commandSender;

        private readonly IEventPublisherAsync _eventPublisherAsync;
        private readonly IEventPublisher _eventPublisher;

        private readonly IQueryProcessorAsync _queryProcessorAsync;
        private readonly IQueryProcessor _queryProcessor;

        public Dispatcher(ICommandSenderAsync commandSenderAsync,
            ICommandSender commandSender,
            IEventPublisherAsync eventPublisherAsync,
            IEventPublisher eventPublisher,
            IQueryProcessorAsync queryProcessorAsync, 
            IQueryProcessor queryProcessor)
        {
            _commandSenderAsync = commandSenderAsync;
            _commandSender = commandSender;
            _eventPublisherAsync = eventPublisherAsync;
            _eventPublisher = eventPublisher;
            _queryProcessorAsync = queryProcessorAsync;
            _queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public async Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            await _commandSenderAsync.SendAsync(command);
        }

        /// <inheritdoc />
        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            _commandSender.Send(command);
        }

        /// <inheritdoc />
        public async Task SendAsync<TCommand, TAggregate>(TCommand command) where TCommand : IDomainCommand where TAggregate : IAggregateRoot
        {
            await _commandSenderAsync.SendAsync<TCommand, TAggregate>(command);
        }

        /// <inheritdoc />
        public void Send<TCommand, TAggregate>(TCommand command) where TCommand : IDomainCommand where TAggregate : IAggregateRoot
        {
            _commandSender.Send<TCommand, TAggregate>(command);
        }

        /// <inheritdoc />
        public async Task SendAndPublishAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            await _commandSenderAsync.SendAndPublishAsync(command);
        }

        /// <inheritdoc />
        public void SendAndPublish<TCommand>(TCommand command) where TCommand : ICommand
        {
            _commandSender.SendAndPublish(command);
        }

        /// <inheritdoc />
        public async Task SendAndPublishAsync<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand 
            where TAggregate : IAggregateRoot
        {
            await _commandSenderAsync.SendAndPublishAsync<TCommand, TAggregate>(command);
        }

        /// <inheritdoc />
        public void SendAndPublish<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand 
            where TAggregate : IAggregateRoot
        {
            _commandSender.SendAndPublish<TCommand, TAggregate>(command);
        }

        /// <inheritdoc />
        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            await _eventPublisherAsync.PublishAsync(@event);
        }

        /// <inheritdoc />
        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            _eventPublisher.Publish(@event);
        }

        /// <inheritdoc />
        public async Task<TResult> GetResultAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            return await _queryProcessorAsync.ProcessAsync<TQuery, TResult>(query);
        }

        /// <inheritdoc />
        public TResult GetResult<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            return _queryProcessor.Process<TQuery, TResult>(query);
        }
    }
}