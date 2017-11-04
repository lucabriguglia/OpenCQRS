using System;
using Weapsy.Mediator.Dependencies;
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

        public CommandSender(IResolver resolver,
            IEventPublisher eventPublisher)
        {
            _resolver = resolver;
            _eventPublisher = eventPublisher;
        }

        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandHandler = _resolver.Resolve<ICommandHandler<TCommand>>();

            if (commandHandler == null)
                throw new ApplicationException($"No handler found for command '{command.GetType().FullName}'");

            commandHandler.Handle(command);
        }

        public void SendAndPublish<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandHandler = _resolver.Resolve<ICommandHandlerWithEvents<TCommand>>();

            if (commandHandler == null)
                throw new ApplicationException($"No handler found for command '{command.GetType().FullName}'");

            var events = commandHandler.Handle(command);

            foreach (var @event in events)
            {
                var concreteEvent = EventFactory.CreateConcreteEvent(@event);
                _eventPublisher.Publish(concreteEvent);
            }
        }
    }
}
