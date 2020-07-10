using System;
using System.Threading.Tasks;
using Kledex.Domain;
using Kledex.Models;

namespace Kledex.Services
{
    public class DomainService : IDomainService
    {
        private readonly IStoreProvider _storeProvider;

        public DomainService(IStoreProvider storeProvider)
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