using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IDomainStore _domainStore;
        private readonly Options _options;

        private bool PublishEvents(ICommand command) => command.PublishEvents ?? _options.PublishEvents;

        public DomainCommandSender(IHandlerResolver handlerResolver,
            IEventPublisher eventPublisher,  
            IEventFactory eventFactory,
            IDomainStore domainStore,
            IOptions<Options> options)
        {
            _handlerResolver = handlerResolver;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
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

            foreach (var @event in events)
            {
                @event.Update(command);               
            }

            await _domainStore.SaveAsync<TAggregate>(command.AggregateRootId, command, events);

            if (PublishEvents(command))
            {
                foreach (var @event in events)
                {
                    var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                    await _eventPublisher.PublishAsync(concreteEvent);
                }
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

            foreach (var @event in events)
            {
                @event.Update(command);
            }

            _domainStore.Save<TAggregate>(command.AggregateRootId, command, events);

            if (PublishEvents(command))
            {
                foreach (var @event in events)
                {
                    var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                    _eventPublisher.Publish(concreteEvent);
                }
            }
        }
    }
}
