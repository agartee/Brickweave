using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Brickweave.Samples.WebApp.Tests.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Brickweave.Samples.WebApp.Tests.Fixtures
{
    public class WebApiFixture : IDisposable
    {
        private const string IDENTITY_AUTHORITY = "https://server";
        private readonly IConfiguration _config;

        public WebApiFixture()
        {
            _config = new ConfigurationBuilder()
                .AddUserSecrets<WebApiFixture>()
                .AddEnvironmentVariables()
                .Build();

            ApiServer = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
        }
        
        public TestServer ApiServer { get; }

        public async Task<string> GetAuthorizationToken()
        {
            var client = new HttpClient();
            var response = await client.PostAsync(
                "https://gartee.auth0.com/oauth/token",
                new
                {
                    client_id = _config["authentication:client_id"],
                    client_secret = _config["authentication:client_secret"],
                    audience = _config["authentication:audience"],
                    grant_type = _config["authentication:grant_type"]
                }.ToStringContent());

            var json = await response.Content.ReadAsStringAsync();
            var result = json.ToJObject();

            return result.SelectToken("access_token").Value<string>();
        }

        public HttpClient CreateApiClient(string token = null)
        {
            var client = ApiServer.CreateClient();

            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        public void Dispose()
        {
            ApiServer.Dispose();
        }
    }
}
