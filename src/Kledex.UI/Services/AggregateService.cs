using System;
using System.Threading.Tasks;
using Kledex.Domain;
using Kledex.UI.Models;

namespace Kledex.UI.Services
{
    public class AggregateService : IAggregateService
    {
        private readonly IStoreProvider _storeProvider;

        public AggregateService(IStoreProvider storeProvider)
        {
            _storeProvider = storeProvider;
        }

        public async Task<AggregateModel> GetAggregateAsync(Guid aggregateId)
        {
            var events = await _storeProvider.GetEventsAsync(aggregateId);
            var aggregate = new AggregateModel(events);
            return aggregate;
        }
    }
}