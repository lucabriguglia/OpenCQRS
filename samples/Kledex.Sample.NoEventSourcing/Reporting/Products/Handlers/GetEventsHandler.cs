using System;
using System.Threading.Tasks;
using Kledex.Domain;
using Kledex.Queries;
using Kledex.Sample.NoEventSourcing.Reporting.Products.Models;

namespace Kledex.Sample.NoEventSourcing.Reporting.Products.Handlers
{
    public class GetEventsHandler : IQueryHandlerAsync<GetEvents, ProductAggregateModel>
    {
        private readonly IEventStore _eventStore;

        public GetEventsHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<ProductAggregateModel> RetrieveAsync(GetEvents query)
        {
            var events = await _eventStore.GetEventsAsync(query.ProductId);
            var aggregate = Activator.CreateInstance<ProductAggregateModel>();
            aggregate.AddEvents(events);
            return aggregate;
        }
    }
}
