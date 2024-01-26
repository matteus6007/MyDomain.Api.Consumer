
using MyDomain.Api.Consumer.App.Models;

namespace MyDomain.Api.Consumer.App
{
    public interface IMyDomainClient
    {
        Task<string?> GetHealthAsync();
        Task<MyDomainDto?> GetMyDomainAsync(Guid id);
    }
}