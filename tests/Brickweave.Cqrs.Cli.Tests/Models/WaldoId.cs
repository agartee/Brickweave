using System;
using System.Collections.Generic;

namespace Brickweave.Cqrs.Cli.Tests.Models
{
    public class WaldoId
    {
        public WaldoId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public override bool Equals(object obj)
        {
            return obj is WaldoId id &&
                   Value.Equals(id.Value);
        }

        public override int GetHashCode()
        {
            return -1937169414 + EqualityComparer<Guid>.Default.GetHashCode(Value);
        }
    }
}
