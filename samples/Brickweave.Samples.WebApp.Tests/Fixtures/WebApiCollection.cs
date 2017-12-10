using Xunit;

namespace Brickweave.Samples.WebApp.Tests.Fixtures
{
    [CollectionDefinition("WebApi Acceptance")]
    public class WebApiCollection : ICollectionFixture<WebApiFixture>
    {
    }
}