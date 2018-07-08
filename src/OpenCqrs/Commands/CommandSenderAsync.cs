using System.Threading.Tasks;
using OpenCqrs.Dependencies;
using OpenCqrs.Domain;
using OpenCqrs.Events;

namespace OpenCqrs.Commands
{
    /// <summary>
    /// CommandSenderAsync
    /// </summary>
    /// <seealso cref="T:OpenCqrs.Commands.ICommandSenderAsync" />
    public class CommandSenderAsync : BaseCommandSender, ICommandSenderAsync
    {
        private readonly IEventPublisherAsync _eventPublisherAsync;
        private readonly IEventFactory _eventFactory;
        private readonly IEventStore _eventStore;
        private readonly ICommandStore _commandStore;

        public CommandSenderAsync(IResolver resolver,
            IEventPublisherAsync eventPublisherAsync,
            IEventFactory eventFactory,
            IEventStore eventStore,
            ICommandStore commandStore)
            : base(resolver)
        {
            _eventPublisherAsync = eventPublisherAsync;
            _eventFactory = eventFactory;
            _eventStore = eventStore;
            _commandStore = commandStore;
        }

        /// <inheritdoc />
        public Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            return GetHandler<ICommandHandlerAsync<TCommand>>(command).HandleAsync(command);
        }

        /// <inheritdoc />
        public async Task SendAsync<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand 
            where TAggregate : IAggregateRoot
        {
            var handler = GetHandler<ICommandHandlerWithDomainEventsAsync<TCommand>>(command);

            await _commandStore.SaveCommandAsync<TAggregate>(command);

            var events = await handler.HandleAsync(command);

            foreach (var @event in events)
            {
                @event.CommandId = command.Id;
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                await _eventStore.SaveEventAsync<TAggregate>((IDomainEvent)concreteEvent);
            }
        }

        /// <inheritdoc />
        public async Task SendAndPublishAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = GetHandler<ICommandHandlerWithEventsAsync<TCommand>>(command);

            var events = await handler.HandleAsync(command);

            foreach (var @event in events)
            {
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                await _eventPublisherAsync.PublishAsync(concreteEvent);
            }
        }

        /// <inheritdoc />
        public async Task SendAndPublishAsync<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand
            where TAggregate : IAggregateRoot
        {
            var handler = GetHandler<ICommandHandlerWithDomainEventsAsync<TCommand>>(command);

            await _commandStore.SaveCommandAsync<TAggregate>(command);

            var events = await handler.HandleAsync(command);

            foreach (var @event in events)
            {
                @event.CommandId = command.Id;
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                await _eventStore.SaveEventAsync<TAggregate>((IDomainEvent)concreteEvent);
                await _eventPublisherAsync.PublishAsync(concreteEvent);
            }
        }
    }
}
