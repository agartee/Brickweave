using System;

namespace Brickweave.Cqrs.Cli.Tests.Models
{
    public class BarId
    {
        public BarId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }
    }
}
