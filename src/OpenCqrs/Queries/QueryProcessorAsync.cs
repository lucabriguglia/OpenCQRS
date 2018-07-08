using System.Threading.Tasks;
using OpenCqrs.Dependencies;

namespace OpenCqrs.Queries
{
    public class QueryProcessorAsync : BaseQueryProcessor, IQueryProcessorAsync
    {
        public QueryProcessorAsync(IResolver resolver) : base(resolver)
        {
        }

        /// <inheritdoc />
        public Task<TResult> ProcessAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            return GetHandler<IQueryHandlerAsync<TQuery, TResult>>(query).RetrieveAsync(query);
        }
    }
}