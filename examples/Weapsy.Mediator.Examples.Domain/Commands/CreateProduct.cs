using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.Examples.Domain.Commands
{
    public class CreateProduct : DomainCommand
    {
        public string Title { get; set; }
    }
}
