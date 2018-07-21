using System.Collections.Generic;

namespace Brickweave.Cqrs.Cli.Tests.Models
{
    public class ListBars : IQuery
    {
        public ListBars(IEnumerable<KeyValuePair<string, object>> attributes)
        {
            Attributes = attributes;
        }

        public IEnumerable<KeyValuePair<string, object>> Attributes { get; }
    }
}
