namespace OpenCqrs.Commands
{
    public class Command : ICommand
    {
        public bool? PublishEvents { get; set; }
    }
}
