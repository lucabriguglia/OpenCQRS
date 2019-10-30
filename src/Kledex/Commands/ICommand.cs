using System.Collections.Generic;

namespace Kledex.Commands
{
    public interface ICommand
    {
        bool? PublishEvents { get; set; }
        IDictionary<string, object> Properties { get; set; }
    }
}
