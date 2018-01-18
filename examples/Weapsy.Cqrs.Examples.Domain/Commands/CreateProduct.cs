using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Examples.Domain.Commands
{
    public class CreateProduct : DomainCommand
    {
        public string Title { get; set; }
    }
}
