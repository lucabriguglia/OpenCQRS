using System;

namespace Kledex.Commands
{
    public interface ICommand
    {
        string UserId { get; set; }
        string Source { get; set; }
        DateTime TimeStamp { get; set; }
        bool? PublishEvents { get; set; }
    }
}
