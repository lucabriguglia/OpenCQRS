using Kledex.Dependencies;
using System.Threading.Tasks;

namespace Kledex.Queries
{
    internal abstract class BaseQueryHandlerWrapper<TResult>
    {
        protected static THandler GetHandler<THandler>(IHandlerResolver handlerResolver)
        {
            return handlerResolver.ResolveHandler<THandler>();
        }

        public abstract Task<TResult> HandleAsync(IQuery<TResult> query, IHandlerResolver serviceFactory);
        public abstract TResult Handle(IQuery<TResult> query, IHandlerResolver serviceFactory);
    }

    internal class QueryHandlerWrapper<TQuery, TResult> : BaseQueryHandlerWrapper<TResult>
        where TQuery : IQuery<TResult>
    {
        public override Task<TResult> HandleAsync(IQuery<TResult> query, IHandlerResolver handlerResolver)
        {
            var handler = GetHandler<IQueryHandlerAsync<TQuery, TResult>>(handlerResolver);
            return handler.HandleAsync((TQuery)query);
        }

        public override TResult Handle(IQuery<TResult> query, IHandlerResolver handlerResolver)
        {
            var handler = GetHandler<IQueryHandler<TQuery, TResult>>(handlerResolver);
            return handler.Handle((TQuery)query);
        }
    }
}
