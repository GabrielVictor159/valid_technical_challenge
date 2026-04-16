namespace Store.Api.DTOs.Requests.Order;

public record OrderSearchRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; init; } = null;
}
