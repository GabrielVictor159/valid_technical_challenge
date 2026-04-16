
namespace Store.Application.Abstractions.DTOs.Responses.Order;

public record CreateOrderResponse(long Id, string NumberOrder, DateTime CreatedDate);