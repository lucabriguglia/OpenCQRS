using System;
using System.Threading.Tasks;
using Kledex.Domain;
using Kledex.Models;

namespace Kledex.Services
{
    public interface IDomainService
    {
        Task<AggregateModel> GetAggregateAsync(Guid aggregateId);
        Task SaveStoreDataAsync(SaveStoreData request);
    }
}
