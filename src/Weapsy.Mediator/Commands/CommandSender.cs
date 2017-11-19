using System;
using Weapsy.Mediator.Dependencies;
using Weapsy.Mediator.Domain;
using Weapsy.Mediator.Events;

namespace Weapsy.Mediator.Commands
{
    /// <inheritdoc />
    /// <summary>
    /// CommandSender
    /// </summary>
    /// <seealso cref="T:Weapsy.Mediator.Commands.ICommandSender" />
    public class CommandSender : ICommandSender
    {
        private readonly IResolver _resolver;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore _eventStore;
        private readonly IEventFactory _eventFactory;

        public CommandSender(IResolver resolver,
            IEventPublisher eventPublisher, 
            IEventStore eventStore, 
            IEventFactory eventFactory)
        {
            _resolver = resolver;
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
            _eventFactory = eventFactory;
        }

        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandHandler = _resolver.Resolve<ICommandHandler<TCommand>>();

            if (commandHandler == null)
                throw new ApplicationException($"No handler of type ICommandHandler<TCommand> found for command '{command.GetType().FullName}'");

            commandHandler.Handle(command);
        }

        public void SendAndPublish<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandHandler = _resolver.Resolve<ICommandHandlerWithEvents<TCommand>>();

            if (commandHandler == null)
                throw new ApplicationException($"No handler of type ICommandHandlerWithEvents<TCommand> found for command '{command.GetType().FullName}'");

            var events = commandHandler.Handle(command);

            foreach (var @event in events)
            {
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                _eventPublisher.Publish(concreteEvent);
            }
        }

        public void SendAndPublish<TCommand, TAggregate>(TCommand command) where TCommand : IDomainCommand where TAggregate : IAggregateRoot
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandHandler = _resolver.Resolve<IDomainCommandHandler<TCommand>>();

            if (commandHandler == null)
                throw new ApplicationException($"No handler of type IDomainCommandHandler<TCommand> found for command '{command.GetType().FullName}'");

            var events = commandHandler.Handle(command);

            foreach (var @event in events)
            {
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                _eventStore.SaveEvent<TAggregate>((IDomainEvent)concreteEvent);
                _eventPublisher.Publish(concreteEvent);
            }
        }
    }
}
