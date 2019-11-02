using System;

namespace Kledex.Commands
{
    public class Command : ICommand
    {
        public string UserId { get; set; }
        public string Source { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public bool? PublishEvents { get; set; }
    }
}
