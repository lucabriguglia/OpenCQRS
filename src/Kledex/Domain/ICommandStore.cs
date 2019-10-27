using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kledex.Domain
{
    public interface ICommandStore
    {
        /// <summary>
        /// Saves the command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        Task SaveCommandAsync(IDomainCommand command);

        /// <summary>
        /// Gets the commands asynchronously.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<IDomainCommand>> GetCommandsAsync(Guid aggregateId);

        /// <summary>
        /// Saves the command.
        /// </summary>
        /// <param name="command">The command.</param>
        void SaveCommand(IDomainCommand command);

        /// <summary>
        /// Gets the commands.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        IEnumerable<IDomainCommand> GetCommands(Guid aggregateId);      
    }

    public class DefaultCommandStore : ICommandStore
    {
        public IEnumerable<IDomainCommand> GetCommands(Guid aggregateId)
        {
            throw new NotImplementedException(Consts.StoreRequiredMessage);
        }

        public Task<IEnumerable<IDomainCommand>> GetCommandsAsync(Guid aggregateId)
        {
            throw new NotImplementedException(Consts.StoreRequiredMessage);
        }

        public void SaveCommand(IDomainCommand command)
        {
            throw new NotImplementedException(Consts.StoreRequiredMessage);
        }

        public Task SaveCommandAsync(IDomainCommand command)
        {
            throw new NotImplementedException(Consts.StoreRequiredMessage);
        }
    }
}
