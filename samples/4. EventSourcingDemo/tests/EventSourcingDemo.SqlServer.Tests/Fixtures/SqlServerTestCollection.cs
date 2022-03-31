using Xunit;

namespace EventSourcingDemo.SqlServer.Tests.Fixtures
{
    [CollectionDefinition("SqlServerTestCollection")]
    public class SqlServerTestCollection : ICollectionFixture<SqlServerTestFixture>
    {
    }
}
