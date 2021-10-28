using Xunit;

namespace Brickweave.Cqrs.SqlServer.Tests.Fixtures
{
    [CollectionDefinition("SqlServerTestCollection")]
    public class SqlServerTestCollection : ICollectionFixture<SqlServerFixture>
    {
    }
}
