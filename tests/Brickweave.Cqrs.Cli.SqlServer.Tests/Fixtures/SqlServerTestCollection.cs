using Xunit;

namespace Brickweave.Cqrs.Cli.SqlServer.Tests.Fixtures
{
    [CollectionDefinition("SqlServerTestCollection")]
    public class SqlServerTestCollection : ICollectionFixture<SqlServerFixture>
    {
    }
}
