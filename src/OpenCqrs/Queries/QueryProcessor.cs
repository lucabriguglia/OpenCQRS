using OpenCqrs.Dependencies;

namespace OpenCqrs.Queries
{
    public class QueryProcessor : BaseQueryProcessor, IQueryProcessor
    {
        public QueryProcessor(IResolver resolver) : base(resolver)
        {
        }

        /// <inheritdoc />
        public TResult Process<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            return GetHandler<IQueryHandler<TQuery, TResult>>(query).Retrieve(query);
        }
    }
}