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
            {
                throw new ArgumentNullException(nameof(command));
            }

            var handler = _handlerResolver.ResolveHandler(command, typeof(IDomainCommandHandlerAsync<>));
            var handleMethod = handler.GetType().GetMethod("HandleAsync");
            var events = await (Task<IEnumerable<IDomainEvent>>)handleMethod.Invoke(handler, new object[] { command });

            var concreteEvents = new List<IDomainEvent>();

            foreach (var @event in events)
            {
                @event.Update(command);
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                concreteEvents.Add(concreteEvent);
            }

            await _domainStore.SaveAsync<TAggregate>(command, concreteEvents);

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
            {
                throw new ArgumentNullException(nameof(command));
            }

            var handler = _handlerResolver.ResolveHandler(command, typeof(IDomainCommandHandler<>));
            var handleMethod = handler.GetType().GetMethod("Handle");
            var events = (IEnumerable<IDomainEvent>)handleMethod.Invoke(handler, new object[] { command });

            var concreteEvents = new List<IDomainEvent>();

            foreach (var @event in events)
            {
                @event.Update(command);
                var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                concreteEvents.Add(concreteEvent);
            }

            _domainStore.Save<TAggregate>(command, concreteEvents);

            if (PublishEvents(command))
            {
                _eventPublisher.Publish(concreteEvents);
            }
        }
    }
}
