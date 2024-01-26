using AutoFixture.Xunit2;

using MyDomain.Api.Consumer.App.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using PactNet;
using PactNet.Infrastructure.Outputters;

using Xunit.Abstractions;

namespace MyDomain.Api.Consumer.App.Tests
{
    public class ConsumerPactTests
    {
        private const string _authorizedToken = "Authorized";

        private readonly IPactBuilderV3 _pact;

        public ConsumerPactTests(ITestOutputHelper output)
        {
            var config = new PactConfig
            {
                PactDir = "../../../pacts/",
                LogLevel = PactLogLevel.Debug,
                Outputters = new List<IOutput> { new XUnitOutput(output), new ConsoleOutput() },

                DefaultJsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };

            var provider = Environment.GetEnvironmentVariable("PACT_PROVIDER") ?? "mydomain-api";

            var pact = Pact.V3("mydomain-consumer", provider, config);

            // the pact builder is created in the constructor so it's unique to each test
            _pact = pact.WithHttpInteractions();
        }

        [Fact]
        public async Task GetHealthCheck_WhenCalled_ShouldReturnOk()
        {
            // Arrange
            _pact
                .UponReceiving("a request to get health check status")
                .WithRequest(HttpMethod.Get, "/v1/healthcheck")
                .WillRespond()
                .WithStatus(System.Net.HttpStatusCode.OK)
                .WithJsonBody(new { status = "Healthy" });

            // Assert
            await _pact.VerifyAsync(async ctx =>
            {
                var client = new MyDomainClient(new HttpClient { BaseAddress = ctx.MockServerUri });
                var health = await client.GetHealthAsync();

                Assert.NotNull(health);
            });
        }

        [Theory]
        [AutoData]
        public async Task GetMyDomain_WhenCalled_AndIsAuthorized_ShouldReturnOK(MyDomainDto dto)
        {
            // Arrange
            _pact
                .UponReceiving("get mydomain with valid auth token")
                .WithRequest(HttpMethod.Get, $"/v1/mydomains/{dto.Id}")
                .WithHeader("Authorization", $"Bearer {_authorizedToken}")
                .WillRespond()
                .WithStatus(System.Net.HttpStatusCode.OK)
                .WithJsonBody(dto);

            // Assert
            await _pact.VerifyAsync(async ctx =>
            {
                var httpClient = new HttpClient { BaseAddress = ctx.MockServerUri };
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authorizedToken}");

                var client = new MyDomainClient(httpClient);
                var myDomain = await client.GetMyDomainAsync(dto.Id);

                Assert.NotNull(myDomain);
                Assert.Equal(dto, myDomain);
            });
        }

        [Theory]
        [AutoData]
        public async Task GetMyDomain_WhenCalled_AndMissingAuthToken_ShouldReturnUnauthorized(Guid id)
        {
            // Arrange
            _pact
                .UponReceiving("get mydomain with no auth token")
                .WithRequest(HttpMethod.Get, $"/v1/mydomains/{id}")
                .WillRespond()
                .WithStatus(System.Net.HttpStatusCode.Unauthorized);

            // Assert
            await _pact.VerifyAsync(async ctx =>
            {
                var client = new MyDomainClient(new HttpClient { BaseAddress = ctx.MockServerUri });

                await Assert.ThrowsAsync<Exception>(() => client.GetMyDomainAsync(id));
            });
        }
    }
}