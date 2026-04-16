

using Store.Application.Abstractions.Interfaces.Commands;

namespace Store.Application.Commands.Order.ProcessOrder;

public record ProcessOrderCommand(long OrderId) : ICommand;
