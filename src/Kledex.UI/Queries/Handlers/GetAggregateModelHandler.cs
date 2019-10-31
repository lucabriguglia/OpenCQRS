using System.Threading.Tasks;
using Kledex.Domain;
using Kledex.Queries;
using Kledex.UI.Models;

namespace Kledex.UI.Queries.Handlers
{
    public class GetAggregateModelHandler : IQueryHandlerAsync<GetAggregateModel, AggregateModel>
    {
        private readonly IDomainStore _domainStore;

        public GetAggregateModelHandler(IDomainStore domainStore)
        {
            _domainStore = domainStore;
        }

        public async Task<AggregateModel> HandleAsync(GetAggregateModel query)
        {
            var events = await _domainStore.GetEventsAsync(query.AggregateRootId);
            var aggregate = new AggregateModel(events);
            return aggregate;
        }
    }
}
