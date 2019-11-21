using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kledex.Commands
{
    public abstract class SequenceCommand : ISequenceCommand
    {
        private readonly List<ICommand> _commands = new List<ICommand>();
        public ReadOnlyCollection<ICommand> Commands => _commands.AsReadOnly();

        /// <summary>
        /// Adds the command to the new command sequence collection.
        /// </summary>
        /// <param name="command">The command.</param>
        protected void AddCommand(ICommand command)
        {
            _commands.Add(command);
        }
    }
}
