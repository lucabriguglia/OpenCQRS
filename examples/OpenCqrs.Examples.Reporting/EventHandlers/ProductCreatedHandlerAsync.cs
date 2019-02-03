using System.Threading.Tasks;
using OpenCqrs.Abstractions.Events;
using OpenCqrs.Examples.Domain.Events;

namespace OpenCqrs.Examples.Reporting.EventHandlers
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
