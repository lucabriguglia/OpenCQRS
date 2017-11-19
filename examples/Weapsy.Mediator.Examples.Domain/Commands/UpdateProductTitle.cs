using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.Examples.Domain.Commands
{
    public class UpdateProductTitle : DomainCommand
    {
        public string Title { get; set; }
    }
}
