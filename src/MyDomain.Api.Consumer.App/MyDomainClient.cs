using System.Text.Json;

using MyDomain.Api.Consumer.App.Models;

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

        public async Task<MyDomainDto?> GetMyDomainAsync(Guid id)
        {
            var response = await _client.GetAsync($"/v1/mydomains/{id}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                return JsonSerializer.Deserialize<MyDomainDto>(content, serializerOptions);
            }

            throw new Exception($"Non-succssful status code {response.StatusCode} returned");
        }
    }
}