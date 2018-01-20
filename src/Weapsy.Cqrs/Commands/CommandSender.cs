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
        private readonly IRepository<IAggregateRoot> _repository;

        public CommandSender(IResolver resolver,
            IEventPublisher eventPublisher,  
            IEventFactory eventFactory, 
            IRepository<IAggregateRoot> repository)
        {
            _resolver = resolver;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _repository = repository;
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

            var commandHandler = _resolver.Resolve<ICommandHandlerWithAggregate<TCommand>>();

            if (commandHandler == null)
                throw new ApplicationException($"No handler of type ICommandHandlerWithAggregate<TCommand> found for command '{command.GetType().FullName}'");

            var aggregateRoot = commandHandler.Handle(command);
            _repository.Save(aggregateRoot);

            foreach (var @event in aggregateRoot.Events)
            {
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                _eventPublisher.Publish(concreteEvent);
            }
        }
    }
}
