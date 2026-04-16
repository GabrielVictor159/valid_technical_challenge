using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Operation
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public OrderStatusEnum Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Message { get; set; } = string.Empty;


    #region NavigationProperties
    public Order? Order { get; set; }
    #endregion
}
