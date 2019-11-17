using System.Threading.Tasks;
using Kledex.Domain;
using Kledex.Queries;
using Kledex.UI.Models;

namespace Kledex.UI.Queries.Handlers
{
    public class GetAggregateModelHandler : IQueryHandlerAsync<GetAggregateModel, AggregateModel>
    {
        private readonly IStoreProvider _storeProvider;

        public GetAggregateModelHandler(IStoreProvider storeProvider)
        {
            _storeProvider = storeProvider;
        }

        public async Task<AggregateModel> HandleAsync(GetAggregateModel query)
        {
            var events = await _storeProvider.GetEventsAsync(query.AggregateRootId);
            var aggregate = new AggregateModel(events);
            return aggregate;
        }
    }
}
