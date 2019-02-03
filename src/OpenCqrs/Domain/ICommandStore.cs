using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Domain
{
    public interface ICommandStore
    {
        /// <summary>
        /// Saves the command asynchronously.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        Task SaveCommandAsync<TAggregate>(IDomainCommand command)
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Gets the commands asynchronously.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<DomainCommand>> GetCommandsAsync(Guid aggregateId);

        /// <summary>
        /// Saves the command.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        void SaveCommand<TAggregate>(IDomainCommand command) 
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Gets the commands.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        IEnumerable<DomainCommand> GetCommands(Guid aggregateId);      
    }
}
