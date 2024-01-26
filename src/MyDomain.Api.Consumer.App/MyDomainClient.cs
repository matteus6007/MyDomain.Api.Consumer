namespace MyDomain.Api.Consumer.App
{
    public class MyDomainClient(HttpClient client) : IMyDomainClient
    {
        private readonly HttpClient _client = client;

        public async Task<string?> GetHealthAsync()
        {
            var response = await _client.GetAsync("/v1/healthcheck");
            var content = await response.Content.ReadAsStringAsync();

            return content;
        }
    }
}