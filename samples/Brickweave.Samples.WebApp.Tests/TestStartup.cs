using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Samples.WebApp.Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IHostingEnvironment env) : base(env)
        {
        }

        protected override void ConfigureSecurity(IServiceCollection services)
        {
            // configure in test fixture
        }
    }
}
