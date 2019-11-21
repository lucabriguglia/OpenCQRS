using System.Collections.ObjectModel;

namespace Kledex.Commands
{
    public interface ISequenceCommand
    {
        ReadOnlyCollection<ICommand> Commands { get; }
    }
}
