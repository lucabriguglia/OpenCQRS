using OpenCqrs.Domain;

namespace OpenCqrs.Examples.Domain.Commands
{
    public class UpdateProductTitle : DomainCommand
    {
        public string Title { get; set; }
    }
}
