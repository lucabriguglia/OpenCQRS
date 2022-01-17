using System.Collections.ObjectModel;

namespace Kledex.Commands
{
    public interface ICommandSequence
    {
        ReadOnlyCollection<ICommand> Commands { get; }
    }
}
