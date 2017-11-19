using System.Threading.Tasks;
using Weapsy.Mediator.Commands;
using Weapsy.Mediator.Domain;
using Weapsy.Mediator.Events;
using Weapsy.Mediator.Queries;

namespace Weapsy.Mediator
{
    /// <inheritdoc />
    /// <summary>
    /// Mediator
    /// </summary>
    /// <seealso cref="T:Weapsy.Mediator.IMediator" />
    public class Mediator : IMediator
    {
        private readonly ICommandSenderAsync _commandSenderAsync;
        private readonly ICommandSender _commandSender;

        private readonly IEventPublisherAsync _eventPublisherAsync;
        private readonly IEventPublisher _eventPublisher;

        private readonly IQueryDispatcherAsync _queryDispatcherAsync;
        private readonly IQueryDispatcher _queryDispatcher;

        public Mediator(ICommandSenderAsync commandSenderAsync,
            ICommandSender commandSender,
            IEventPublisherAsync eventPublisherAsync,
            IEventPublisher eventPublisher,
            IQueryDispatcherAsync queryDispatcherAsync, 
            IQueryDispatcher queryDispatcher)
        {
            _commandSenderAsync = commandSenderAsync;
            _commandSender = commandSender;
            _eventPublisherAsync = eventPublisherAsync;
            _eventPublisher = eventPublisher;
            _queryDispatcherAsync = queryDispatcherAsync;
            _queryDispatcher = queryDispatcher;
        }

        public async Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            await _commandSenderAsync.SendAsync(command);
        }

        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            _commandSender.Send(command);
        }

        public async Task SendAndPublishAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            await _commandSenderAsync.SendAndPublishAsync(command);
        }

        public void SendAndPublish<TCommand>(TCommand command) where TCommand : ICommand
        {
            _commandSender.SendAndPublish(command);
        }

        public async Task SendAndPublishAsync<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand 
            where TAggregate : IAggregateRoot
        {
            await _commandSenderAsync.SendAndPublishAsync<TCommand, TAggregate>(command);
        }

        public void SendAndPublish<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand 
            where TAggregate : IAggregateRoot
        {
            _commandSender.SendAndPublish<TCommand, TAggregate>(command);
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            await _eventPublisherAsync.PublishAsync(@event);
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            _eventPublisher.Publish(@event);
        }

        public async Task<TResult> GetResultAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            return await _queryDispatcherAsync.DispatchAsync<TQuery, TResult>(query);
        }

        public TResult GetResult<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            return _queryDispatcher.Dispatch<TQuery, TResult>(query);
        }
    }
}