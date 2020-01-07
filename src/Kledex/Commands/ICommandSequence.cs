using System;
using System.Collections.ObjectModel;

namespace Kledex.Commands
{
    [Obsolete("Please use ISaga instead.")]
    public interface ICommandSequence
    {
        ReadOnlyCollection<ICommand> Commands { get; }
    }
}
