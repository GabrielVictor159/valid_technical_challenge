namespace Store.Api.DTOs.Requests.Order;

public record CreateOrderRequest(string NumberOrder, decimal TotalPrice);