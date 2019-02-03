using OpenCqrs.Abstractions.Commands;

namespace OpenCqrs.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}