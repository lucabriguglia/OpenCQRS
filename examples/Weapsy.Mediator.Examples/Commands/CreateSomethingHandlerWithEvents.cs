using System.Collections.Generic;
using Weapsy.Mediator.Commands;
using Weapsy.Mediator.Events;

namespace Weapsy.Mediator.Examples.Commands
{
    public class CreateSomethingHandlerWithEvents : ICommandHandlerWithEvents<CreateSomething>
    {
        public IEnumerable<IEvent> Handle(CreateSomething command)
        {
            throw new System.NotImplementedException();
        }
    }
}
