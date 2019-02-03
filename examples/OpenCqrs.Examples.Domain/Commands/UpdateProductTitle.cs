using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Examples.Domain.Commands
{
    public class UpdateProductTitle : DomainCommand
    {
        public string Title { get; set; }
    }
}
