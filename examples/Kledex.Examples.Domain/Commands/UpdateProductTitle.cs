using Kledex.Domain;

namespace Kledex.Examples.Domain.Commands
{
    public class UpdateProductTitle : DomainCommand
    {
        public string Title { get; set; }
    }
}
