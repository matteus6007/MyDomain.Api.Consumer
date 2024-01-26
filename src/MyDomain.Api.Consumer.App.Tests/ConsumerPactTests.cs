using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using PactNet;
using PactNet.Infrastructure.Outputters;

using Xunit.Abstractions;

namespace MyDomain.Api.Consumer.App.Tests
{
    public class ConsumerPactTests
    {
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
    }
}