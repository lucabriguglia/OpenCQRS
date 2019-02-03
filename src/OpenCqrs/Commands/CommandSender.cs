using System;
using System.Threading.Tasks;
using OpenCqrs.Abstractions.Commands;
using OpenCqrs.Abstractions.Domain;
using OpenCqrs.Dependencies;
using OpenCqrs.Domain;
using OpenCqrs.Events;

namespace OpenCqrs.Commands
{
    /// <summary>
    /// CommandSender
    /// </summary>
    /// <seealso cref="T:OpenCqrs.Commands.ICommandSender" />
    public class CommandSender : ICommandSender
    {
        private readonly IHandlerResolver _handlerResolver;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IEventStore _eventStore;
        private readonly ICommandStore _commandStore;
        
        public CommandSender(IHandlerResolver handlerResolver,
            IEventPublisher eventPublisher,  
            IEventFactory eventFactory,
            IEventStore eventStore, 
            ICommandStore commandStore)
        {
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _eventStore = eventStore;
            _commandStore = commandStore;
            _handlerResolver = handlerResolver;
        }

        /// <inheritdoc />
        public Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _handlerResolver.ResolveHandler<ICommandHandlerAsync<TCommand>>();

            return handler.HandleAsync(command);
        }

        /// <inheritdoc />
        public async Task SendAsync<TCommand, TAggregate>(TCommand command)
            where TCommand : IDomainCommand
            where TAggregate : IAggregateRoot
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _handlerResolver.ResolveHandler<ICommandHandlerWithDomainEventsAsync<TCommand>>();

            await _commandStore.SaveCommandAsync<TAggregate>(command);

            var events = await handler.HandleAsync(command);

            foreach (var @event in events)
            {
                @event.Update(command);
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                await _eventStore.SaveEventAsync<TAggregate>((IDomainEvent)concreteEvent, command.ExpectedVersion);
            }
        }

        /// <inheritdoc />
        public async Task SendAndPublishAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _handlerResolver.ResolveHandler<ICommandHandlerWithEventsAsync<TCommand>>();

            var events = await handler.HandleAsync(command);

            foreach (var @event in events)
            {
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                await _eventPublisher.PublishAsync(concreteEvent);
            }
        }

        /// <inheritdoc />
        public async Task SendAndPublishAsync<TCommand, TAggregate>(TCommand command)
            where TCommand : IDomainCommand
            where TAggregate : IAggregateRoot
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _handlerResolver.ResolveHandler<ICommandHandlerWithDomainEventsAsync<TCommand>>();

            await _commandStore.SaveCommandAsync<TAggregate>(command);

            var events = await handler.HandleAsync(command);

            foreach (var @event in events)
            {
                @event.Update(command);
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                await _eventStore.SaveEventAsync<TAggregate>((IDomainEvent)concreteEvent, command.ExpectedVersion);
                await _eventPublisher.PublishAsync(concreteEvent);
            }
        }

        /// <summary>
        /// Sends the specified command.
        /// The command handler must implement OpenCqrs.Commands.ICommandHandler.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        /// <inheritdoc />
        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _handlerResolver.ResolveHandler<ICommandHandler<TCommand>>();
            
            handler.Handle(command);
        }

        /// <inheritdoc />
        public void Send<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand 
            where TAggregate : IAggregateRoot
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _handlerResolver.ResolveHandler<ICommandHandlerWithDomainEvents<TCommand>>();

            _commandStore.SaveCommand<TAggregate>(command);

            var events = handler.Handle(command);

            foreach (var @event in events)
            {
                @event.Update(command);
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                _eventStore.SaveEvent<TAggregate>((IDomainEvent)concreteEvent, command.ExpectedVersion);
            }
        }

        /// <inheritdoc />
        public void SendAndPublish<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _handlerResolver.ResolveHandler<ICommandHandlerWithEvents<TCommand>>();

            var events = handler.Handle(command);

            foreach (var @event in events)
            {
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                _eventPublisher.Publish(concreteEvent);
            }
        }

        /// <inheritdoc />
        public void SendAndPublish<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand 
            where TAggregate : IAggregateRoot
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _handlerResolver.ResolveHandler<ICommandHandlerWithDomainEvents<TCommand>>();

            _commandStore.SaveCommand<TAggregate>(command);

            var events = handler.Handle(command);

            foreach (var @event in events)
            {
                @event.Update(command);
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                _eventStore.SaveEvent<TAggregate>((IDomainEvent)concreteEvent, command.ExpectedVersion);
                _eventPublisher.Publish(concreteEvent);
            }
        }
    }
}
