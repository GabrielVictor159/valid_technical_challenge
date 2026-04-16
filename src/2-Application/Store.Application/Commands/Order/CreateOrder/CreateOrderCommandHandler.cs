using FluentValidation;
using Mapster;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Store.Application.Abstractions.DTOs.Responses.Order;
using Store.Application.Abstractions.Interfaces.Commands;
using Store.Application.Abstractions.Interfaces.Repositories;
using Store.Domain.Contracts.Order;

namespace Store.Application.Commands.Order.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private readonly IUnitOfWork _uow;
    private readonly IBus _bus;

    public CreateOrderCommandHandler(
        ILogger<CreateOrderCommandHandler> logger,
        IUnitOfWork uow,
        IBus bus)
    {
        _logger = logger;
        _uow = uow;
        _bus = bus;
    }

    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Usuário {UserId} ({Email}) iniciando criação do pedido: {NumberOrder}",
            request.User?.UserId, request.User?.Email, request.NumberOrder);

        try
        {
            await _uow.BeginTransactionAsync();

            _logger.LogInformation("Verificando se ja existe um pedido Numero {NumberOrder}", request.NumberOrder);

            var existingOrder = (await _uow.Repository<Domain.Entities.Order>()
                .FindAsync(e => e.NumberOrder.Equals(request.NumberOrder))).FirstOrDefault();

            if (existingOrder != null)
                throw new ValidationException($"Ja existe um pedido com o numero {request.NumberOrder}");

            var order = request.Adapt<Store.Domain.Entities.Order>();
            order.Status = Domain.Enums.OrderStatusEnum.RECEBIDO;

            order.Operations.Add(new Domain.Entities.Operation()
            {
                Status = Domain.Enums.OrderStatusEnum.RECEBIDO,
                Message = $"Pedido recebido pelo usuário {request.User?.UserName} e encaminhado para a fila de processamento"
            });

            await _uow.Repository<Store.Domain.Entities.Order>().AddAsync(order);
            await _uow.CommitAsync();

            _logger.LogInformation("Pedido {NumberOrder} (ID {Id}) salvo no banco.", order.NumberOrder, order.Id);

            await _uow.CommitTransactionAsync();

            var eventMessage = order.Adapt<CreateOrderEvent>();
            eventMessage.OrderId = order.Id;

            await _bus.Send(eventMessage);

            _logger.LogInformation("Pedido {NumberOrder} processado e evento publicado com sucesso.", order.NumberOrder);

            return order.Adapt<CreateOrderResponse>();
        }
        catch (Exception ex)
        {
            await _uow.RollbackTransactionAsync();

            _logger.LogError(ex, "Falha crítica ao criar o pedido {NumberOrder}. Transação revertida",
                request.NumberOrder);

            throw;
        }
    }
}