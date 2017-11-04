using System.Threading.Tasks;
using Weapsy.Mediator.Queries;

namespace Weapsy.Mediator.Examples.Queries
{
    public class GetSomethingHandlerAsync : IQueryHandlerAsync<GetSomething, Something>
    {
        public async Task<Something> RetrieveAsync(GetSomething query)
        {
            return await Task.FromResult(new Something{Name = "Something"});
        }
    }
}
