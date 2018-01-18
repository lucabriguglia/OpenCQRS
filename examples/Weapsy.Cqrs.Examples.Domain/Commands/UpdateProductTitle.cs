using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Examples.Domain.Commands
{
    public class UpdateProductTitle : DomainCommand
    {
        public string Title { get; set; }
    }
}
