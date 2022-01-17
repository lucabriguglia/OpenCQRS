using System;

namespace Kledex.Exceptions
{
    public class QueryException : Exception
    {
        public QueryException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
