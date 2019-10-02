using System.Threading.Tasks;
using Kledex.Domain;
using Kledex.Queries;
using Kledex.UI.Models;

namespace Kledex.UI.Queries.Handlers
{
    public class GetAggregateModelHandler : IQueryHandlerAsync<GetAggregateModel, AggregateModel>
    {
        private readonly IEventStore _eventStore;

        public GetAggregateModelHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<AggregateModel> HandleAsync(GetAggregateModel query)
        {
            var events = await _eventStore.GetEventsAsync(query.AggregateRootId);
            var aggregate = new AggregateModel(events);
            return aggregate;
        }
    }
}
