using System.Threading.Tasks;
using OpenCqrs.Abstractions.Events;
using OpenCqrs.Examples.Domain.Events;

namespace OpenCqrs.Examples.Reporting.EventHandlers
{
    public class ProductCreatedBusMessageHandlerAsync : IEventHandlerAsync<ProductCreatedBusMessage>
    {
        public async Task HandleAsync(ProductCreatedBusMessage @event)
        {
            await Task.CompletedTask;

            var model = new ProductViewModel
            {
                Id = @event.AggregateRootId,
                Title = @event.Title
            };

            FakeReadDatabase.Products.Add(model);
        }
    }
}
