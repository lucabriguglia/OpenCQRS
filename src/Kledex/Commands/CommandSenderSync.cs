using System;
using System.Collections.Generic;
using Kledex.Domain;

namespace Kledex.Commands
{
    /// <inheritdoc />
    public partial class CommandSender : ICommandSender
    {
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
                    var concreteEvent = _objectFactory.CreateConcreteObject(@event);
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
    }
}
