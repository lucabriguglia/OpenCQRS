using System.Threading.Tasks;
using Kledex.Events;
using Kledex.Examples.Domain.Events;

namespace Kledex.Examples.Reporting.EventHandlers
{
    public class ProductTitleUpdatedHandlerAsync : IEventHandlerAsync<ProductTitleUpdated>
    {
        public async Task HandleAsync(ProductTitleUpdated @event)
        {
            await Task.CompletedTask;

            var model = FakeReadDatabase.Products.Find(x => x.Id == @event.AggregateRootId);
            model.Title = @event.Title;
        }
    }
}
