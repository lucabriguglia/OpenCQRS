using System.Threading.Tasks;
using Weapsy.Cqrs.Events;
using Weapsy.Cqrs.Examples.Domain.Events;

namespace Weapsy.Cqrs.Examples.Reporting.EventHandlers
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
