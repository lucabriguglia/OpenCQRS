using System.Threading.Tasks;
using Weapsy.Mediator.Commands;

namespace Weapsy.Mediator.Examples.Commands
{
    public class CreateSomethingHandlerAsync : ICommandHandlerAsync<CreateSomething>
    {
        public Task HandleAsync(CreateSomething command)
        {
            throw new System.NotImplementedException();
        }
    }
}
