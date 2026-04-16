using Store.Domain.Enums;

namespace Store.Application.Abstractions.DTOs.Responses.Operations;

public record SearchOperationResponse(OrderStatusEnum Status, string Message, DateTime CreatedDate);
