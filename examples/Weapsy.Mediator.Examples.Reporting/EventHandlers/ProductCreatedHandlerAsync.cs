using System.Threading.Tasks;
using Weapsy.Mediator.Events;
using Weapsy.Mediator.Examples.Domain.Events;

namespace Weapsy.Mediator.Examples.Reporting.EventHandlers
{
    public class ProductCreatedHandlerAsync : IEventHandlerAsync<ProductCreated>
    {
        public async Task HandleAsync(ProductCreated @event)
        {
            await Task.CompletedTask;

            var model = new ProductViewModel
            {
                Id = @event.AggregateId,
                Title = @event.Title
            };

            FakeReadDatabase.Products.Add(model);
        }
    }
}
