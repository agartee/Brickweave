using System;
using Brickweave.Domain;

namespace Brickweave.Samples.Domain.Sandbox.Models
{
    public class FooId : ValueObject<Guid>
    {
        public FooId(Guid value) : base(value)
        {
        }
    }
}
