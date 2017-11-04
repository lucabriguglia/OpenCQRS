using Weapsy.Mediator.Commands;

namespace Weapsy.Mediator.Examples.Commands
{
    public class CreateSomethingHandler : ICommandHandler<CreateSomething>
    {
        public void Handle(CreateSomething command)
        {
            throw new System.NotImplementedException();
        }
    }
}
