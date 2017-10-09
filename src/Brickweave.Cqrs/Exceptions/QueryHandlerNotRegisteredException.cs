using System;

namespace Brickweave.Cqrs.Exceptions
{
    public class QueryHandlerNotRegisteredException : Exception
    {
        private const string MESSAGE = "Handler not registered for query, {0}";

        public QueryHandlerNotRegisteredException(IQuery query)
            : base(string.Format(MESSAGE, query.GetType()))
        {
            Query = query;
        }

        public IQuery Query { get; }
    }
}