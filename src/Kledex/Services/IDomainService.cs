using System;
using System.Threading.Tasks;
using Kledex.Models;

namespace Kledex.Services
{
    public interface IDomainService
    {
        Task<AggregateModel> GetAggregateAsync(Guid aggregateId);
    }
}
