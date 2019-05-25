using Kledex.Domain;

namespace Kledex.Examples.Domain.Commands
{
    public class CreateProduct : DomainCommand
    {
        public string Title { get; set; }
    }
}
