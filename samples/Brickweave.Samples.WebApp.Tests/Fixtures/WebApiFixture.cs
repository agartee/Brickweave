using System;
using System.Net.Http;
using System.Threading.Tasks;
using Brickweave.Samples.WebApp.Tests.IdentityServer;
using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Samples.WebApp.Tests.Fixtures
{
    public class WebApiFixture : IDisposable
    {
        private const string IDENTITY_AUTHORITY = "https://server";

        public WebApiFixture()
        {
            IdentityServer = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<IdentityServerStartup>());

            ApiServer = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<TestStartup>()
                .ConfigureServices(services =>
                {
                    services.AddAuthentication("Bearer")
                        .AddIdentityServerAuthentication(options =>
                        {
                            options.Authority = IDENTITY_AUTHORITY;
                            options.ApiName = "brickweave_api";
                            options.JwtBackChannelHandler = IdentityServer.CreateHandler();
                            options.IntrospectionDiscoveryHandler = IdentityServer.CreateHandler();
                            options.IntrospectionBackChannelHandler = IdentityServer.CreateHandler();
                        });
                }));
        }

        public TestServer IdentityServer { get; }
        public TestServer ApiServer { get; }

        public async Task<string> GetAuthorizationToken()
        {
            var discoveryClient = new DiscoveryClient(IDENTITY_AUTHORITY, IdentityServer.CreateHandler());
            var discoveryResponse = await discoveryClient.GetAsync();

            var tokenClient = new TokenClient(discoveryResponse.TokenEndpoint,
                "test_client", "secret", IdentityServer.CreateHandler());
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync(
                "brickweave_api");

            return tokenResponse.AccessToken;
        }

        public HttpClient CreateApiClient(string token = null)
        {
            var client = ApiServer.CreateClient();

            if (token != null)
                client.SetBearerToken(token);

            return client;
        }

        public void Dispose()
        {
            IdentityServer.Dispose();
            ApiServer.Dispose();
        }
    }
}
