namespace MyDomain.Api.Consumer.App.Models
{
    public record MyDomainDto(
        Guid Id,
        string Name,
        string Description,
        DateTime CreatedOn,
        DateTime UpdatedOn);
}