using System;
using System.Threading.Tasks;
using Kledex.UI.Models;

namespace Kledex.UI.Services
{
    public interface IAggregateService
    {
        Task<AggregateModel> GetAggregateAsync(Guid aggregateId);
    }
}
