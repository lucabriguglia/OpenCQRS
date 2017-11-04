using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Mediator.Commands;
using Weapsy.Mediator.Events;

namespace Weapsy.Mediator.Examples.Commands
{
    public class CreateSomethingHandlerWithEventsAsync : ICommandHandlerWithEventsAsync<CreateSomething>
    {
        public Task<IEnumerable<IEvent>> HandleAsync(CreateSomething command)
        {
            throw new System.NotImplementedException();
        }
    }
}
