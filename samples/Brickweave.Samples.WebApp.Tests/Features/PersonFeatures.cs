using System;
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
    public class PersonFeatures
    {
        private readonly WebApiFixture _fixture;

        public PersonFeatures(WebApiFixture fixture)
        {
            _fixture = fixture;
        }

        [Scenario]
        public void CreatePerson(string firstName, string lastName,
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

            "When a person is created through the API"
                .x(async () =>
                {
                    response = await client.PostAsync(
                        "/person/new", new
                        {
                            firstName,
                            lastName
                        }.ToStringContent());
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

        [Scenario]
        public void GetPerson(Guid id, string firstName, string lastName,
            HttpClient client, HttpResponseMessage response)
        {
            "Given the client is authenticated"
                .x(async () =>
                {
                    var token = await _fixture.GetAuthorizationToken();
                    client = _fixture.CreateApiClient(token);
                });

            "And a person exists with a first name of 'Adam' and last name of 'Gartee'"
                .x(async () =>
                {
                    response = await client.PostAsync(
                        "/person/new", new
                        {
                            firstName,
                            lastName
                        }.ToStringContent());

                    var json = await response.Content.ReadAsStringAsync();
                    id = new Guid(json.ToJObject().SelectToken("id").Value<string>());
                });

            $"When a person is fetched through the API by ID ({id})"
                .x(async () =>
                {
                    response = await client.GetAsync(
                        $"/person/{id}");
                });

            "Then the response status code is 200 (OK)"
                .x(() => response.StatusCode.Should().Be(HttpStatusCode.OK));

            "And the response payload is the existing person"
                .x(async () =>
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = json.ToJObject();

                    result.SelectToken("id").Value<string>().Should().Be(id.ToString());
                    result.SelectToken("name.firstName").Value<string>().Should().Be(firstName);
                    result.SelectToken("name.lastName").Value<string>().Should().Be(lastName);
                });
        }
    }
}
