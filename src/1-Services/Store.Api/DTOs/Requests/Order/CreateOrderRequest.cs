namespace Store.Api.DTOs.Requests.Order;

public record CreateOrderRequest(string NumberOrder, string Client, decimal TotalPrice);