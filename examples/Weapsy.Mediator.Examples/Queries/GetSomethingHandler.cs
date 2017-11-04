using Weapsy.Mediator.Queries;

namespace Weapsy.Mediator.Examples.Queries
{
    public class GetSomethingHandler : IQueryHandler<GetSomething, Something>
    {
        public Something Retrieve(GetSomething query)
        {
            return new Something();
        }
    }
}
