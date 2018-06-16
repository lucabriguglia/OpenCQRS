using System.Threading.Tasks;
using Weapsy.Cqrs.Events;
using Weapsy.Cqrs.Examples.Domain.Events;

namespace Weapsy.Cqrs.Examples.Reporting.EventHandlers
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
