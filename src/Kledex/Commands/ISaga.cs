using System.Collections.ObjectModel;

namespace Kledex.Commands
{
    public interface ISaga
    {
        ReadOnlyCollection<ICommand> Commands { get; }
    }
}
