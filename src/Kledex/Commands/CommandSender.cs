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
    public class CommandSender : ICommandSender
    {
        private readonly IHandlerResolver _handlerResolver;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IDomainStore _domainStore;
        private readonly Options _options;

        private bool PublishEvents(ICommand command) => command.PublishEvents ?? _options.PublishEvents;

        public CommandSender(IHandlerResolver handlerResolver,
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
        public async Task SendAsync<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            IEnumerable<IEvent> events;

            if (command is IDomainCommand<IAggregateRoot> domainCommand)
            {
                var handler = _handlerResolver.ResolveCommandHandler(command, typeof(ICommandHandlerAsync<>));
                var handleMethod = handler.GetType().GetMethod("HandleAsync");
                events = await (Task<IEnumerable<IEvent>>)handleMethod.Invoke(handler, new object[] { command });

                foreach (var @event in (IEnumerable<IDomainEvent>)events)
                {
                    @event.Update(domainCommand);
                }

                await _domainStore.SaveAsync(GetAggregateType(domainCommand), domainCommand.AggregateRootId, domainCommand, (IEnumerable<IDomainEvent>)events);
            }
            else
            {
                var handler = _handlerResolver.ResolveHandler<ICommandHandlerAsync<TCommand>>();
                events = await handler.HandleAsync(command);
            }

            if (PublishEvents(command))
            {
                foreach (var @event in events)
                {
                    var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                    await _eventPublisher.PublishAsync(concreteEvent);
                }
            }
        }

        public async Task<TResult> SendAsync<TResult>(IDomainCommand<IAggregateRoot, TResult> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var handler = _handlerResolver.ResolveCommandHandler(command, typeof(ICommandHandlerAsync2<>));
            var handleMethod = handler.GetType().GetMethod("HandleAsync");
            var response = await(Task<CommandResponse>)handleMethod.Invoke(handler, new object[] { command });

            foreach (var @event in (IEnumerable<IDomainEvent>)response.Events)
            {
                @event.Update(command);
            }

            await _domainStore.SaveAsync(GetAggregateType(command), command.AggregateRootId, command, (IEnumerable<IDomainEvent>)response.Events);

            return (TResult)response.Result;
        }

        /// <inheritdoc />
        public void Send<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            IEnumerable<IEvent> events;

            if (command is IDomainCommand<IAggregateRoot> domainCommand)
            {
                var handler = _handlerResolver.ResolveCommandHandler(command, typeof(ICommandHandler<>));
                var handleMethod = handler.GetType().GetMethod("Handle");
                events = (IEnumerable<IEvent>)handleMethod.Invoke(handler, new object[] { command });

                foreach (var @event in (IEnumerable<IDomainEvent>)events)
                {
                    @event.Update(domainCommand);
                }

                _domainStore.Save(GetAggregateType(domainCommand), domainCommand.AggregateRootId, domainCommand, (IEnumerable<IDomainEvent>)events);
            }
            else
            {
                var handler = _handlerResolver.ResolveHandler<ICommandHandler<TCommand>>();
                events = handler.Handle(command);
            }

            if (PublishEvents(command))
            {
                foreach (var @event in events)
                {
                    var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                    _eventPublisher.Publish(concreteEvent);
                }
            }
        }

        private Type GetAggregateType(IDomainCommand domainCommand)
        {
            var commandType = domainCommand.GetType();
            var commandInterface = commandType.GetInterfaces()[1];
            var aggregateType = commandInterface.GetGenericArguments().FirstOrDefault();
            return aggregateType;
        }
    }
}
