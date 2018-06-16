using System;
using System.Threading.Tasks;
using Weapsy.Cqrs.Dependencies;
using Weapsy.Cqrs.Domain;
using Weapsy.Cqrs.Events;

namespace Weapsy.Cqrs.Commands
{
    /// <inheritdoc />
    /// <summary>
    /// CommandSenderAsync
    /// </summary>
    /// <seealso cref="T:Weapsy.Cqrs.Commands.ICommandSenderAsync" />
    public class CommandSenderAsync : ICommandSenderAsync
    {
        private readonly IResolver _resolver;
        private readonly IEventPublisherAsync _eventPublisherAsync;
        private readonly IEventFactory _eventFactory;
        private readonly IEventStore _eventStore;
        private readonly ICommandStore _commandStore;

        public CommandSenderAsync(IResolver resolver,
            IEventPublisherAsync eventPublisherAsync, 
            IEventFactory eventFactory,
            IEventStore eventStore, 
            ICommandStore commandStore)
        {
            _resolver = resolver;
            _eventPublisherAsync = eventPublisherAsync;
            _eventFactory = eventFactory;
            _eventStore = eventStore;
            _commandStore = commandStore;
        }

        /// <inheritdoc />
        public async Task SendAsync<TCommand>(TCommand command) 
            where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _resolver.Resolve<ICommandHandlerAsync<TCommand>>();

            if (handler == null)
                throw new ApplicationException($"No handler of type CommandHandlerAsync<TCommand> found for command '{command.GetType().FullName}'");

            await handler.HandleAsync(command);
        }

        /// <inheritdoc />
        public async Task SendAsync<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand 
            where TAggregate : IAggregateRoot
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            await _commandStore.SaveCommandAsync<TAggregate>(command);

            var handler = _resolver.Resolve<ICommandHandlerWithAggregateAsync<TCommand>>();

            if (handler == null)
                throw new ApplicationException($"No handler of type ICommandHandlerWithAggregateAsync<TCommand> found for command '{command.GetType().FullName}'");

            var aggregateRoot = await handler.HandleAsync(command);

            foreach (var @event in aggregateRoot.Events)
            {
                @event.CommandId = command.Id;
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                await _eventStore.SaveEventAsync<TAggregate>((IDomainEvent)concreteEvent);
            }
        }

        /// <inheritdoc />
        public async Task SendAndPublishAsync<TCommand>(TCommand command) 
            where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _resolver.Resolve<ICommandHandlerWithEventsAsync<TCommand>>();

            if (handler == null)
                throw new ApplicationException($"No handler of type ICommandHandlerWithEventsAsync<TCommand> found for command '{command.GetType().FullName}'");

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
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            await _commandStore.SaveCommandAsync<TAggregate>(command);

            var handler = _resolver.Resolve<ICommandHandlerWithAggregateAsync<TCommand>>();

            if (handler == null)
                throw new ApplicationException($"No handler of type ICommandHandlerWithAggregateAsync<TCommand> found for command '{command.GetType().FullName}'");

            var aggregateRoot = await handler.HandleAsync(command);

            foreach (var @event in aggregateRoot.Events)
            {
                @event.CommandId = command.Id;
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                await _eventStore.SaveEventAsync<TAggregate>((IDomainEvent)concreteEvent);
                await _eventPublisherAsync.PublishAsync(concreteEvent);
            }
        }
    }
}
