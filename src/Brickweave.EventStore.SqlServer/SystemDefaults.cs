using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;

namespace Brickweave.EventStore.SqlServer
{
    internal static class SystemDefaults
    {
        public static IDocumentSerializer DocumentSerializer { get; set; }
        public static IAggregateFactory AggregateFactory { get; set; }
    }
}
