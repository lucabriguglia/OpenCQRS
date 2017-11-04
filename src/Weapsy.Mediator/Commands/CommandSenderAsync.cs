using System;
using System.Threading.Tasks;
using Weapsy.Mediator.Dependencies;
using Weapsy.Mediator.Events;

namespace Weapsy.Mediator.Commands
{
    /// <inheritdoc />
    /// <summary>
    /// CommandSenderAsync
    /// </summary>
    /// <seealso cref="T:Weapsy.Mediator.Commands.ICommandSenderAsync" />
    public class CommandSenderAsync : ICommandSenderAsync
    {
        private readonly IResolver _resolver;
        private readonly IEventPublisherAsync _eventPublisher;

        public CommandSenderAsync(IResolver resolver,
            IEventPublisherAsync eventPublisher)
        {
            _resolver = resolver;
            _eventPublisher = eventPublisher;
        }

        public async Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandHandler = _resolver.Resolve<ICommandHandlerAsync<TCommand>>();

            if (commandHandler == null)
                throw new ApplicationException($"No handler found for command '{command.GetType().FullName}'");

            await commandHandler.HandleAsync(command);
        }

        public async Task SendAndPublishAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandHandler = _resolver.Resolve<ICommandHandlerWithEventsAsync<TCommand>>();

            if (commandHandler == null)
                throw new ApplicationException($"No handler found for command '{command.GetType().FullName}'");

            var events = await commandHandler.HandleAsync(command);

            foreach (var @event in events)
            {
                var concreteEvent = EventFactory.CreateConcreteEvent(@event);
                await _eventPublisher.PublishAsync(concreteEvent);
            }
        }
    }
}
