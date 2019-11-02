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
        public async Task SendAsync(ICommand command)
        {
            await ProcessAsync(command);
        }

        /// <inheritdoc />
        public async Task<TResult> SendAsync<TResult>(ICommand command)
        {
            var response = await ProcessAsync(command);

            return response.Result != null ? (TResult)response.Result : default;
        }

        /// <inheritdoc />
        public void Send(ICommand command)
        {
            Process(command);
        }

        /// <inheritdoc />
        public TResult Send<TResult>(ICommand command)
        {
            var response = Process(command);

            return response.Result != null ? (TResult)response.Result : default;
        }

        private async Task<CommandResponse> ProcessAsync(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var handler = _handlerResolver.ResolveCommandHandler(command, typeof(ICommandHandlerAsync<>));
            var handleMethod = handler.GetType().GetMethod("HandleAsync");
            var response = await (Task<CommandResponse>)handleMethod.Invoke(handler, new object[] { command });

            if (command is IDomainCommand domainCommand)
            {
                foreach (var @event in (IEnumerable<IDomainEvent>)response.Events)
                {
                    @event.Update(domainCommand);
                }

                await _domainStore.SaveAsync(GetAggregateType(domainCommand), domainCommand.AggregateRootId, domainCommand, (IEnumerable<IDomainEvent>)response.Events);
            }

            if (PublishEvents(command))
            {
                foreach (var @event in response.Events)
                {
                    var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                    await _eventPublisher.PublishAsync(concreteEvent);
                }
            }

            return response;
        }

        private CommandResponse Process(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var handler = _handlerResolver.ResolveCommandHandler(command, typeof(ICommandHandler<>));
            var handleMethod = handler.GetType().GetMethod("Handle");
            var response = (CommandResponse)handleMethod.Invoke(handler, new object[] { command });

            if (command is IDomainCommand domainCommand)
            {
                foreach (var @event in (IEnumerable<IDomainEvent>)response.Events)
                {
                    @event.Update(domainCommand);
                }

                _domainStore.Save(GetAggregateType(domainCommand), domainCommand.AggregateRootId, domainCommand, (IEnumerable<IDomainEvent>)response.Events);
            }

            if (PublishEvents(command))
            {
                foreach (var @event in response.Events)
                {
                    var concreteEvent = _eventFactory.CreateConcreteEvent(@event);
                    _eventPublisher.Publish(concreteEvent);
                }
            }

            return response;
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
