using System.Threading.Tasks;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Cqrs.Examples.Reporting.Queries
{
    public class GetProductHandlerAsync : IQueryHandlerAsync<GetProduct, ProductViewModel>
    {
        public async Task<ProductViewModel> RetrieveAsync(GetProduct query)
        {
            await Task.CompletedTask;

            var model = FakeReadDatabase.Products.Find(x => x.Id == query.Id);
            return model;
        }
    }
}
