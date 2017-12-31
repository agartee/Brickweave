using System.Net;
using System.Net.Http;
using Brickweave.Samples.WebApp.Tests.Extensions;
using Brickweave.Samples.WebApp.Tests.Fixtures;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xbehave;
using Xunit;

namespace Brickweave.Samples.WebApp.Tests.Features
{
    [Collection("WebApi Acceptance")]
    [Trait("Subject", "Integration")]
    public class CommandFeatures
    {
        private readonly WebApiFixture _fixture;

        public CommandFeatures(WebApiFixture fixture)
        {
            _fixture = fixture;
        }

        [Scenario]
        public void CreatePerson(string firstName, string lastName, string birthDate, 
            HttpClient client, HttpResponseMessage response)
        {
            "Given the client is authenticated"
                .x(async () =>
                {
                    var token = await _fixture.GetAuthorizationToken();
                    client = _fixture.CreateApiClient(token);
                });

            "And the model specifies a first name of 'Adam'"
                .x(() => firstName = "Adam");

            "and a last name of 'Gartee'"
                .x(() => lastName = "Gartee");

            "and a birthDate of '1/1/1980'"
                .x(() => birthDate = "1/1/1980");

            "When a person is created through the CLI endpoint"
                .x(async () =>
                {
                    response = await client.PostAsync(
                        "/command/run", new StringContent($"person create --firstName {firstName} --lastName {lastName} --birthDate {birthDate}"));
                });

            "Then the response status code is 200 (OK)"
                .x(() => response.StatusCode.Should().Be(HttpStatusCode.OK));

            "And the response payload is the created person"
                .x(async () =>
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = json.ToJObject();

                    result.SelectToken("id").Value<string>().Should().NotBeNullOrEmpty();
                    result.SelectToken("name.firstName").Value<string>().Should().Be(firstName);
                    result.SelectToken("name.lastName").Value<string>().Should().Be(lastName);
                });
        }
    }
}
