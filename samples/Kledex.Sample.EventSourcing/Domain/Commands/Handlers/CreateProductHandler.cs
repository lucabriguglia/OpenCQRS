using System.Threading.Tasks;
using Kledex.Commands;

namespace Kledex.Sample.EventSourcing.Domain.Commands.Handlers
{
    public class CreateProductHandler : ICommandHandlerAsync<CreateProduct>
    {
        public async Task<CommandResponse> HandleAsync(CreateProduct command)
        {
            var product = new Product(command.AggregateRootId, command.Name, command.Description, command.Price);

            return await Task.FromResult(new CommandResponse
            {
                Events = product.Events,
                Result = true
            });
        }
    }
}
