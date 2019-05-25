using System.Threading.Tasks;
using Kledex.Events;
using Kledex.Examples.Domain.Events;

namespace Kledex.Examples.Reporting.EventHandlers
{
    public class ProductCreatedHandlerAsync : IEventHandlerAsync<ProductCreated>
    {
        public async Task HandleAsync(ProductCreated @event)
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
