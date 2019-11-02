using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Commands;
using Kledex.Events;

namespace Kledex.Sample.EventSourcing.Domain.Commands.Handlers
{
    public class CreateProductHandler : ICommandHandlerAsync2<CreateProduct>
    {
        public async Task<CommandResponse> HandleAsync(CreateProduct command)
        {
            var product = new Product(command.AggregateRootId, command.Name, command.Description, command.Price);

            var events = await Task.FromResult(product.Events);

            return new CommandResponse
            {
                Events = events,
                Result = true
            };
        }
    }
}
