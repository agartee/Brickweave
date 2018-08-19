using Xunit;

namespace Brickweave.Messaging.ServiceBus.Tests.Fixtures
{
    [CollectionDefinition("SqlServerTestCollection")]
    public class ServiceBusTestCollection : ICollectionFixture<ServiceBusFixture>
    {
    }
}
