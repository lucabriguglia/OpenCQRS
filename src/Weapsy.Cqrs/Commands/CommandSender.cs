using System;
using Weapsy.Cqrs.Dependencies;
using Weapsy.Cqrs.Domain;
using Weapsy.Cqrs.Events;

namespace Weapsy.Cqrs.Commands
{
    /// <inheritdoc />
    /// <summary>
    /// CommandSender
    /// </summary>
    /// <seealso cref="T:Weapsy.Cqrs.Commands.ICommandSender" />
    public class CommandSender : ICommandSender
    {
        private readonly IResolver _resolver;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IEventStore _eventStore;
        private readonly ICommandStore _commandStore;

        public CommandSender(IResolver resolver,
            IEventPublisher eventPublisher,  
            IEventFactory eventFactory,
            IEventStore eventStore, 
            ICommandStore commandStore)
        {
            _resolver = resolver;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _eventStore = eventStore;
            _commandStore = commandStore;
        }

        /// <summary>
        /// Sends the specified command.
        /// The command handler must implement WeapsyCqrs.Commands.ICommandHandler.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        /// <inheritdoc />
        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _resolver.Resolve<ICommandHandler<TCommand>>();

            if (handler == null)
                throw new ApplicationException($"No handler of type WeapsyCqrs.Commands.ICommandHandler<TCommand> found for command '{command.GetType().FullName}'");

            handler.Handle(command);
        }

        /// <inheritdoc />
        public void Send<TCommand, TAggregate>(TCommand command) where TCommand : IDomainCommand where TAggregate : IAggregateRoot
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            _commandStore.SaveCommand<TAggregate>(command);

            var handler = _resolver.Resolve<ICommandHandlerWithAggregate<TCommand>>();

            if (handler == null)
                throw new ApplicationException($"No handler of type WeapsyCqrs.Commands.ICommandHandlerWithAggregate<TCommand> found for command '{command.GetType().FullName}'");

            var aggregateRoot = handler.Handle(command);

            foreach (var @event in aggregateRoot.Events)
            {
                @event.CommandId = command.Id;
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                _eventStore.SaveEvent<TAggregate>((IDomainEvent)concreteEvent);
            }
        }

        /// <inheritdoc />
        public void SendAndPublish<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _resolver.Resolve<ICommandHandlerWithEvents<TCommand>>();

            if (handler == null)
                throw new ApplicationException($"No handler of type WeapsyCqrs.Commands.ICommandHandlerWithEvents<TCommand> found for command '{command.GetType().FullName}'");

            var events = handler.Handle(command);

            foreach (var @event in events)
            {
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                _eventPublisher.Publish(concreteEvent);
            }
        }

        /// <inheritdoc />
        public void SendAndPublish<TCommand, TAggregate>(TCommand command) where TCommand : IDomainCommand where TAggregate : IAggregateRoot
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            _commandStore.SaveCommand<TAggregate>(command);

            var handler = _resolver.Resolve<ICommandHandlerWithAggregate<TCommand>>();

            if (handler == null)
                throw new ApplicationException($"No handler of type WeapsyCqrs.Commands.ICommandHandlerWithAggregate<TCommand> found for command '{command.GetType().FullName}'");

            var aggregateRoot = handler.Handle(command);

            foreach (var @event in aggregateRoot.Events)
            {
                @event.CommandId = command.Id;
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                _eventStore.SaveEvent<TAggregate>((IDomainEvent)concreteEvent);
                _eventPublisher.Publish(concreteEvent);
            }
        }
    }
}
