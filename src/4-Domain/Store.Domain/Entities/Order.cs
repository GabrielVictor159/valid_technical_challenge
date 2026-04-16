using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Order
{
    public long Id { get; set; }
    public string NumberOrder { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public OrderStatusEnum Status { get; set; }
    public decimal TotalPrice { get; set; }

    public ICollection<Operation> Operations { get; set; } = new List<Operation>();

}
