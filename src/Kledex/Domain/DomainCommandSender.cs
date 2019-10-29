using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Commands;
using Kledex.Dependencies;
using Kledex.Events;
using Microsoft.Extensions.Options;
using Options = Kledex.Configuration.Options;

namespace Kledex.Domain
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
        private readonly IDomainStore _domainStore;
        private readonly Options _options;

        private bool PublishEvents(ICommand command) => command.PublishEvents ?? _options.PublishEvents;

        public DomainCommandSender(IHandlerResolver handlerResolver,
            IEventPublisher eventPublisher,  
            IEventFactory eventFactory,
            IAggregateStore aggregateStore,
            ICommandStore commandStore,
            IEventStore eventStore,
            IDomainStore domainStore,
            IOptions<Options> options)
        {
            _handlerResolver = handlerResolver;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _aggregateStore = aggregateStore;
            _commandStore = commandStore;
            _eventStore = eventStore;
            _domainStore = domainStore;
            _options = options.Value;
        }

        /// <inheritdoc />
        public async Task SendAsync<TAggregate>(IDomainCommand<TAggregate> command) 
            where TAggregate : IAggregateRoot
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _handlerResolver.ResolveHandler(command, typeof(IDomainCommandHandlerAsync2<>));
            var handleMethod = handler.GetType().GetMethod("HandleAsync");

            //var aggregateTask = _aggregateStore.SaveAggregateAsync<TAggregate>(command.AggregateRootId);
            //var commandTask = _commandStore.SaveCommandAsync(command);
            //var eventsTask = (Task<IEnumerable<IDomainEvent>>)handleMethod.Invoke(handler, new object[] { command });
            var response = await (Task<HandlerResponse>)handleMethod.Invoke(handler, new object[] { command });

            //await Task.WhenAll(aggregateTask, commandTask, eventsTask);

            //var publishEvents = PublishEvents(command);
            //var events = await eventsTask;

            var concreteEvents = new List<IDomainEvent>();

            foreach (var @event in response.Events)
            {
                @event.Update(command);
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);

                //await _eventStore.SaveEventAsync<TAggregate>((IDomainEvent)concreteEvent, command.ExpectedVersion);

                concreteEvents.Add(concreteEvent);

                //if (publishEvents)
                //    await _eventPublisher.PublishAsync(concreteEvent);
            }

            await _domainStore.SaveAsync<TAggregate>(new SaveStoreData
            {
                Command = command,
                Events = concreteEvents,
                Properties = response.Properties
            });

            if (PublishEvents(command))
            {
                await _eventPublisher.PublishAsync(concreteEvents);
            }
        }

        /// <inheritdoc />
        public void Send<TAggregate>(IDomainCommand<TAggregate> command)
            where TAggregate : IAggregateRoot
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _handlerResolver.ResolveHandler(command, typeof(IDomainCommandHandler<>));
            var handleMethod = handler.GetType().GetMethod("Handle");

            _aggregateStore.SaveAggregate<TAggregate>(command.AggregateRootId);
            _commandStore.SaveCommand(command);
            var events = (IEnumerable<IDomainEvent>)handleMethod.Invoke(handler, new object[] { command });

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
