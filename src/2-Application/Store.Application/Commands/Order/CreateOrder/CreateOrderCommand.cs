using Store.Application.Abstractions.DTOs;
using Store.Application.Abstractions.DTOs.Responses.Order;
using Store.Application.Abstractions.Interfaces.Commands;

namespace Store.Application.Commands.Order.CreateOrder;

public record CreateOrderCommand(string NumberOrder, decimal TotalPrice, UserSession? User) : ICommand<CreateOrderResponse>;
