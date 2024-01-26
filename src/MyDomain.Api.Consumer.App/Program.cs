// See https://aka.ms/new-console-template for more information

using MyDomain.Api.Consumer.App;

var providerUrl = Environment.GetEnvironmentVariable("PROVIDER_URL") ?? "http://localhost:1001/";

var client = new MyDomainClient(new HttpClient { BaseAddress = new Uri(providerUrl) });

var health = await client.GetHealthAsync();

Console.WriteLine(health);