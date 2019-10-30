namespace Kledex.Commands
{
    public interface ICommand
    {
        bool? PublishEvents { get; set; }
    }
}
