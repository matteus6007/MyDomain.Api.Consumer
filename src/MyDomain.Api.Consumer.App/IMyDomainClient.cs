
namespace MyDomain.Api.Consumer.App
{
    public interface IMyDomainClient
    {
        Task<string?> GetHealthAsync();
    }
}