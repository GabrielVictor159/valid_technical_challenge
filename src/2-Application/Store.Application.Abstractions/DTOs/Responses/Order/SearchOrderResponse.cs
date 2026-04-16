using Store.Application.Abstractions.DTOs.Responses.Operations;
using Store.Domain.Enums;

namespace Store.Application.Abstractions.DTOs.Responses.Order;

public class SearchOrderResponse
{ 
    public long Id { get; set; }
    public string NumberOrder { get; set; } = string.Empty;
    public OrderStatusEnum Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public decimal TotalPrice { get; set; }
    public List<SearchOperationResponse> Operations { get; set; } = new List<SearchOperationResponse>(); 
}
