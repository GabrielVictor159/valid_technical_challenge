namespace Store.Domain.Contracts.Order;

public class CreateOrderEvent
{
    public required long OrderId { get; set; }
    public required decimal TotalPrice { get; set; }
}
