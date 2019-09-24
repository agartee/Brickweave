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
    public class CliFeatures
    {
        private readonly WebApiFixture _fixture;

        public CliFeatures(WebApiFixture fixture)
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
                        "/cli/run", new StringContent($"person create --firstName {firstName} --lastName {lastName} --birthDate {birthDate}"));
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
        public void AddPhoneNumber(string personId, string phoneType, string phoneNumber,
            HttpClient client, HttpResponseMessage response)
        {
            "Given the client is authenticated"
                .x(async () =>
                {
                    var token = await _fixture.GetAuthorizationToken();
                    client = _fixture.CreateApiClient(token);
                });

            "and a person was created through the CLI endpoint"
                .x(async () =>
                {
                    var firstName = "Adam";
                    var lastName = "Gartee";
                    var birthDate = "1/1/1980";

                    var createPersonResponse = await client.PostAsync(
                        "/cli/run", new StringContent($"person create --firstName {firstName} --lastName {lastName} --birthDate {birthDate}"));

                    var json = await createPersonResponse.Content.ReadAsStringAsync();
                    var result = json.ToJObject();

                    personId = result.SelectToken("id").Value<string>();
                });

            "when phone numbers are added to the person"
                .x(async () =>
                {
                    phoneNumber = "(555) 555-1111";
                    phoneType = "home";

                    response = await client.PostAsync(
                        "/cli/run", new StringContent($"person phones add --personid {personId} --type {phoneType} --number \"{phoneNumber}\""));
                });

            "Then the response status code is 200 (OK)"
                .x(() => response.StatusCode.Should().Be(HttpStatusCode.OK));

            "And the response payload is the person with the added phones"
                .x(async () =>
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = json.ToJObject();

                    result.SelectToken("id").Value<string>().Should().NotBeNullOrEmpty();
                    result.SelectToken($"phones.[0].number").Value<string>().Should().Be(phoneNumber);
                });
        }

        [Scenario]
        public void AddSingleAttribute(string personId, string key, string value,
            HttpClient client, HttpResponseMessage response)
        {
            "Given the client is authenticated"
                .x(async () =>
                {
                    var token = await _fixture.GetAuthorizationToken();
                    client = _fixture.CreateApiClient(token);
                });

            "and a person was created through the CLI endpoint"
                .x(async () =>
                {
                    var firstName = "Adam";
                    var lastName = "Gartee";
                    var birthDate = "1/1/1980";

                    var createPersonResponse = await client.PostAsync(
                        "/cli/run", new StringContent($"person create --firstName {firstName} --lastName {lastName} --birthDate {birthDate}"));

                    var json = await createPersonResponse.Content.ReadAsStringAsync();
                    var result = json.ToJObject();

                    personId = result.SelectToken("id").Value<string>();
                });

            "when an attribute is added to the person"
                .x(async () =>
                {
                    key = "key";
                    value = "value 1";

                    response = await client.PostAsync(
                        "/cli/run", new StringContent($"person attributes add-single --personid {personId} --key {key} --value \"{value}\""));
                });

            "Then the response status code is 200 (OK)"
                .x(() => response.StatusCode.Should().Be(HttpStatusCode.OK));

            "And the response payload is the person with the added attributes"
                .x(async () =>
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = json.ToJObject();

                    result.SelectToken("id").Value<string>().Should().NotBeNullOrEmpty();
                    result.SelectToken($"attributes.{key}[0]").Value<string>().Should().Be(value);
                });
        }

        [Scenario]
        public void AddMultipleAttributes(string personId, string key, string value1, string value2,
            HttpClient client, HttpResponseMessage response)
        {
            "Given the client is authenticated"
                .x(async () =>
                {
                    var token = await _fixture.GetAuthorizationToken();
                    client = _fixture.CreateApiClient(token);
                });

            "and a person was created through the CLI endpoint"
                .x(async () =>
                {
                    var firstName = "Adam";
                    var lastName = "Gartee";
                    var birthDate = "1/1/1980";

                    var createPersonResponse = await client.PostAsync(
                        "/cli/run", new StringContent($"person create --firstName {firstName} --lastName {lastName} --birthDate {birthDate}"));

                    var json = await createPersonResponse.Content.ReadAsStringAsync();
                    var result = json.ToJObject();

                    personId = result.SelectToken("id").Value<string>();
                });

            "when an attribute with multiple values is added to the person"
                .x(async () =>
                {
                    key = "key";
                    value1 = "value1";
                    value2 = "value2";

                    response = await client.PostAsync(
                        "/cli/run", new StringContent($"person attributes add-multiple --personid {personId} --attributes {key}[={value1}] {key}[={value2}]")); 
                });

            "Then the response status code is 200 (OK)"
                .x(() => response.StatusCode.Should().Be(HttpStatusCode.OK));

            "And the response payload is the person with the added attributes"
                .x(async () =>
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = json.ToJObject();

                    result.SelectToken("id").Value<string>().Should().NotBeNullOrEmpty();
                    result.SelectToken($"attributes.{key}[0]").Value<string>().Should().Be(value1);
                    result.SelectToken($"attributes.{key}[1]").Value<string>().Should().Be(value2);
                });
        }
    }
}
