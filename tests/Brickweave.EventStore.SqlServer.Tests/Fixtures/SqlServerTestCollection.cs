using Xunit;

namespace Brickweave.EventStore.SqlServer.Tests.Fixtures
{
    [CollectionDefinition("SqlServerTestCollection")]
    public class SqlServerTestCollection : ICollectionFixture<SqlServerFixture>
    {
    }
}
