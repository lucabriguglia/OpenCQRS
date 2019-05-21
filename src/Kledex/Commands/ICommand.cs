namespace OpenCqrs.Commands
{
    public interface ICommand
    {
        bool? PublishEvents { get; set; }
    }
}
