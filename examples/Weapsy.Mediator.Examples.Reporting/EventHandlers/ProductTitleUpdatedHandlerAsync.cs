using System.Threading.Tasks;
using Weapsy.Mediator.Events;
using Weapsy.Mediator.Examples.Domain.Events;

namespace Weapsy.Mediator.Examples.Reporting.EventHandlers
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
