using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kledex.Dependencies;
using Kledex.Domain;
using Kledex.Events;
using Kledex.Validation;
using Microsoft.Extensions.Options;
using Options = Kledex.Configuration.Options;

namespace Kledex.Commands
{
    /// <inheritdoc />
    public class CommandSender : ICommandSender
    {
        private readonly IHandlerResolver _handlerResolver;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IStoreProvider _storeProvider;
        private readonly IValidationService _validationService;
        private readonly Options _options;

        private bool ValidateCommand(ICommand command) => command.Validate ?? _options.ValidateCommands;
        private bool PublishEvents(ICommand command) => command.PublishEvents ?? _options.PublishEvents;

        public CommandSender(IHandlerResolver handlerResolver,
            IEventPublisher eventPublisher,  
            IEventFactory eventFactory,
            IStoreProvider storeProvider,
            IValidationService validationService,
            IOptions<Options> options)
        {
            _handlerResolver = handlerResolver;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _storeProvider = storeProvider;
            _validationService = validationService;
            _options = options.Value;
        }

        /// <inheritdoc />
        public async Task SendAsync(ICommand command)
        {
            await ProcessAsync(command, () => GetCommandResponseAsync(command));
        }

        /// <inheritdoc />
        public async Task SendAsync(ICommand command, Func<Task<CommandResponse>> commandHandler)
        {
            await ProcessAsync(command, commandHandler);
        }

        /// <inheritdoc />
        public Task SendAsync(ICommandSequence commandSequence)
        {
            return ProcessCommandSequenceAsync(commandSequence);
        }

        /// <inheritdoc />
        public async Task<TResult> SendAsync<TResult>(ICommand command)
        {
            var response = await ProcessAsync(command, () => GetCommandResponseAsync(command));
            return response?.Result != null ? (TResult)response.Result : default;
        }

        /// <inheritdoc />
        public async Task<TResult> SendAsync<TResult>(ICommand command, Func<Task<CommandResponse>> commandHandler)
        {
            var response = await ProcessAsync(command, commandHandler);
            return response?.Result != null ? (TResult)response.Result : default;
        }

        /// <inheritdoc />
        public async Task<TResult> SendAsync<TResult>(ICommandSequence commandSequence)
        {
            var lastStepReponse = await ProcessCommandSequenceAsync(commandSequence);
            return lastStepReponse?.Result != null ? (TResult)lastStepReponse.Result : default;
        }

        private async Task<CommandResponse> ProcessCommandSequenceAsync(ICommandSequence commandSequence)
        {
            CommandResponse lastStepResponse = null;

            foreach (var command in commandSequence.Commands)
            {
                var response = await ProcessAsync(command, () => GetSequenceCommandResponseAsync(command, lastStepResponse));
                lastStepResponse = response;
            }

            return lastStepResponse;
        }

        private async Task<CommandResponse> ProcessAsync(ICommand command, Func<Task<CommandResponse>> getResponse)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (ValidateCommand(command))
            {
                await _validationService.ValidateAsync(command);
            }

            var response = await getResponse();

            if (response == null)
            {
                return null;
            }

            if (command is IDomainCommand domainCommand)
            {
                foreach (var @event in (IEnumerable<IDomainEvent>)response.Events)
                {
                    @event.Update(domainCommand);
                }

                await _storeProvider.SaveAsync(new SaveStoreData
                {
                    AggregateType = GetAggregateType(domainCommand),
                    AggregateRootId = domainCommand.AggregateRootId,
                    Events = (IEnumerable<IDomainEvent>)response.Events,
                    DomainCommand = domainCommand
                });
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

        private Task<CommandResponse> GetCommandResponseAsync(ICommand command)
        {
            var handler = _handlerResolver.ResolveHandler(command, typeof(ICommandHandlerAsync<>));
            var handleMethod = handler.GetType().GetMethod("HandleAsync", new[] { command.GetType() });
            return (Task<CommandResponse>)handleMethod.Invoke(handler, new object[] { command });
        }

        private Task<CommandResponse> GetSequenceCommandResponseAsync(ICommand command, CommandResponse previousStepResponse)
        {
            var handler = _handlerResolver.ResolveHandler(command, typeof(ISequenceCommandHandlerAsync<>));
            var handleMethod = handler.GetType().GetMethod("HandleAsync", new[] { command.GetType(), typeof(CommandResponse) });
            return (Task<CommandResponse>)handleMethod.Invoke(handler, new object[] { command, previousStepResponse });
        }

        /// <inheritdoc />
        public void Send(ICommand command)
        {
            Process(command, () => GetCommandResponse(command));
        }

        /// <inheritdoc />
        public void Send(ICommand command, Func<CommandResponse> commandHandler)
        {
            Process(command, commandHandler);
        }

        /// <inheritdoc />
        public void Send(ICommandSequence commandSequence)
        {
            ProcessSequenceCommand(commandSequence);
        }

        /// <inheritdoc />
        public TResult Send<TResult>(ICommand command)
        {
            var response = Process(command, () => GetCommandResponse(command));
            return response?.Result != null ? (TResult)response.Result : default;
        }

        /// <inheritdoc />
        public TResult Send<TResult>(ICommand command, Func<CommandResponse> commandHandler)
        {
            var response = Process(command, commandHandler);
            return response?.Result != null ? (TResult)response.Result : default;
        }

        /// <inheritdoc />
        public TResult Send<TResult>(ICommandSequence commandSequence)
        {
            var lastStepReponse = ProcessSequenceCommand(commandSequence);
            return lastStepReponse?.Result != null ? (TResult)lastStepReponse.Result : default;
        }

        private CommandResponse ProcessSequenceCommand(ICommandSequence commandSequence)
        {
            CommandResponse lastStepResponse = null;

            foreach (var command in commandSequence.Commands)
            {
                var response = Process(command, () => GetSequenceCommandResponse(command, lastStepResponse));
                lastStepResponse = response;
            }

            return lastStepResponse;
        }

        private CommandResponse Process(ICommand command, Func<CommandResponse> getResponse)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (ValidateCommand(command))
            {
                _validationService.Validate(command);
            }

            var response = getResponse();

            if (response == null)
            {
                return null;
            }

            if (command is IDomainCommand domainCommand)
            {
                foreach (var @event in (IEnumerable<IDomainEvent>)response.Events)
                {
                    @event.Update(domainCommand);
                }

                _storeProvider.Save(new SaveStoreData
                {
                    AggregateType = GetAggregateType(domainCommand),
                    AggregateRootId = domainCommand.AggregateRootId,
                    Events = (IEnumerable<IDomainEvent>)response.Events,
                    DomainCommand = domainCommand
                });
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

        private CommandResponse GetCommandResponse(ICommand command)
        {
            var handler = _handlerResolver.ResolveHandler(command, typeof(ICommandHandler<>));
            var handleMethod = handler.GetType().GetMethod("Handle", new[] { command.GetType() });
            return (CommandResponse)handleMethod.Invoke(handler, new object[] { command });
        }

        private CommandResponse GetSequenceCommandResponse(ICommand command, CommandResponse previousStepResponse)
        {
            var handler = _handlerResolver.ResolveHandler(command, typeof(ISequenceCommandHandler<>));
            var handleMethod = handler.GetType().GetMethod("Handle", new[] { command.GetType(), typeof(CommandResponse) });
            return (CommandResponse)handleMethod.Invoke(handler, new object[] { command, previousStepResponse });
        }

        private static Type GetAggregateType(IDomainCommand domainCommand)
        {
            var commandType = domainCommand.GetType();
            var commandInterface = commandType.GetInterfaces()[1];
            var aggregateType = commandInterface.GetGenericArguments().FirstOrDefault();
            return aggregateType;
        }
    }
}
