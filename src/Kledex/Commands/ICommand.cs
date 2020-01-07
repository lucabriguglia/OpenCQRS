using System;

namespace Kledex.Commands
{
    public interface ICommand
    {
        Guid Id { get; set; }
        string UserId { get; set; }
        string Source { get; set; }
        DateTime TimeStamp { get; set; }
        bool? ValidateCommand { get; set; }
        bool? PublishEvents { get; set; }
    }
}
