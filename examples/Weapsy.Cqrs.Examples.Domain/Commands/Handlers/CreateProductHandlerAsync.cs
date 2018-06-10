using System.Threading.Tasks;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Examples.Domain.Commands.Handlers
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
