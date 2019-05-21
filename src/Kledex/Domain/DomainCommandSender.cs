using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OpenCqrs.Commands;
using OpenCqrs.Dependencies;
using OpenCqrs.Events;
using Options = OpenCqrs.Configuration.Options;

namespace OpenCqrs.Domain
{
    /// <inheritdoc />
    public class DomainCommandSender : IDomainCommandSender
    {
        private readonly IHandlerResolver _handlerResolver;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IAggregateStore _aggregateStore;
        private readonly ICommandStore _commandStore;
        private readonly IEventStore _eventStore;
        private readonly Options _options;

        private bool PublishEvents(ICommand command) => command.PublishEvents ?? _options.PublishEvents;

        public DomainCommandSender(IHandlerResolver handlerResolver,
            IEventPublisher eventPublisher,  
            IEventFactory eventFactory,
            IAggregateStore aggregateStore,
            ICommandStore commandStore,
            IEventStore eventStore, 
            IOptions<Options> options)
        {
            _handlerResolver = handlerResolver;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _aggregateStore = aggregateStore;
            _commandStore = commandStore;
            _eventStore = eventStore;            
            _options = options.Value;
        }

        /// <inheritdoc />
        public async Task SendAsync<TCommand, TAggregate>(TCommand command)
            where TCommand : IDomainCommand
            where TAggregate : IAggregateRoot
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _handlerResolver.ResolveHandler<IDomainCommandHandlerAsync<TCommand>>();

            var aggregateTask = _aggregateStore.SaveAggregateAsync<TAggregate>(command.AggregateRootId);
            var commandTask = _commandStore.SaveCommandAsync<TAggregate>(command);
            var eventsTask = handler.HandleAsync(command);

            await Task.WhenAll(aggregateTask, commandTask, eventsTask);

            var publishEvents = PublishEvents(command);
            var events = await eventsTask;

            foreach (var @event in events)
            {
                @event.Update(command);
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);

                await _eventStore.SaveEventAsync<TAggregate>((IDomainEvent)concreteEvent, command.ExpectedVersion);

                if (publishEvents)
                    await _eventPublisher.PublishAsync(concreteEvent);
            }
        }

        /// <inheritdoc />
        public void Send<TCommand, TAggregate>(TCommand command) 
            where TCommand : IDomainCommand 
            where TAggregate : IAggregateRoot
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _handlerResolver.ResolveHandler<IDomainCommandHandler<TCommand>>();

            _aggregateStore.SaveAggregate<TAggregate>(command.AggregateRootId);
            _commandStore.SaveCommand<TAggregate>(command);

            var events = handler.Handle(command);

            var publishEvents = PublishEvents(command);

            foreach (var @event in events)
            {
                @event.Update(command);
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);

                _eventStore.SaveEvent<TAggregate>((IDomainEvent)concreteEvent, command.ExpectedVersion);

                if (publishEvents)
                    _eventPublisher.Publish(concreteEvent);
            }
        }
    }
}
