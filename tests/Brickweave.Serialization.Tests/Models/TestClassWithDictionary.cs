using System;
using System.Collections.Generic;

namespace Brickweave.Serialization.Tests.Models
{
    public class TestClassWithDictionary : IHasThings
    {
        public TestClassWithDictionary(Guid id, IDictionary<string, IEnumerable<object>> things)
        {
            Id = id;
            Things = things;
        }

        public Guid Id { get; }
        public IDictionary<string, IEnumerable<object>> Things { get; }
    }

    public interface IHasThings
    {

    }
}
