using System.Threading.Tasks;
using OpenCqrs.Commands;
using OpenCqrs.Domain;

namespace OpenCqrs.Examples.Domain.Commands.Handlers
{
    public class CreateProductHandlerAsync : ICommandHandlerWithAggregateAsync<CreateProduct>
    {
        public async Task<IAggregateRoot> HandleAsync(CreateProduct command)
        {
            await Task.CompletedTask;

            var product = new Product(command.AggregateRootId, command.Title);

            return product;
        }
    }
}
