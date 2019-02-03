using System.Threading.Tasks;
using OpenCqrs.Abstractions.Events;
using OpenCqrs.Examples.Domain.Events;

namespace OpenCqrs.Examples.Reporting.EventHandlers
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
