using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.Interfaces.Commands;
using Store.Application.Abstractions.Interfaces.Repositories;
using Store.Application.Commands.Order.CreateOrder;

namespace Store.Application.Commands.Order.ProcessOrder;

public class ProcessOrderCommandHandler : ICommandHandler<ProcessOrderCommand>
{
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private readonly IUnitOfWork _uow;

    public ProcessOrderCommandHandler(
        ILogger<CreateOrderCommandHandler> logger,
        IUnitOfWork uow)
    {
        _logger = logger;
        _uow = uow;
    }

    public async Task Handle(ProcessOrderCommand request, CancellationToken ct)
    {
        try
        {
            await _uow.BeginTransactionAsync();

            _logger.LogInformation("Obtendo pedido {OrderId} ", request.OrderId);
            var order = await _uow.Repository<Domain.Entities.Order>().GetByIdAsync(request.OrderId);

            if (order == null)
                throw new ArgumentException($"Não foi possivel localizar o processo {request.OrderId}");

            _logger.LogInformation("Salvando status de processamento no pedido {OrderId} ", request.OrderId);

            order.Status = Domain.Enums.OrderStatusEnum.EM_PROCESSAMENTO;
            order.Operations.Add(new Domain.Entities.Operation() 
            { 
                Status = Domain.Enums.OrderStatusEnum.EM_PROCESSAMENTO,
                Message = "Iniciado processamento do pedido"
            });

            await _uow.CommitTransactionAsync();

            await _uow.BeginTransactionAsync();

            _logger.LogInformation("Iniciando fluxo de processamento do pedido {OrderId} ", request.OrderId);

            order.TotalPrice *= 1.27m;
            order.Status = Domain.Enums.OrderStatusEnum.PROCESSADO;
            order.Operations.Add(new Domain.Entities.Operation()
            {
                Status = Domain.Enums.OrderStatusEnum.PROCESSADO,
                Message = "Finalizado processamento do pedido e inserido o imposto de 27%"
            });

            await _uow.CommitTransactionAsync();

            _logger.LogInformation("Finalizado fluxo de processamento do pedido {OrderId} ", request.OrderId);


        }
        catch (Exception ex)
        {
            await _uow.RollbackTransactionAsync();

            _logger.LogError(ex, "Falha crítica ao processar o pedido {OrderId}. Transação revertida",
                request.OrderId);

            try
            {
                _logger.LogInformation("Iniciando fluxo de salvamento de erro no pedido {OrderId} ", request.OrderId);
                await _uow.BeginTransactionAsync();

                var order = await _uow.Repository<Domain.Entities.Order>().GetByIdAsync(request.OrderId);

                if (order != null)
                {
                    order.Status = Domain.Enums.OrderStatusEnum.ERRO;
                    order.Operations.Add(new Domain.Entities.Operation() 
                    { 
                        Status = Domain.Enums.OrderStatusEnum.ERRO, 
                        Message = $"Houve um erro ao processar o pedido, Error = {ex.Message}." 
                    });
                }

                await _uow.CommitTransactionAsync();
            }
            catch (Exception ex2)
            {
                _logger.LogError(ex, "Falha crítica ao salvar o erro no pedido {OrderId}. Transação revertida",
                request.OrderId);
            }
        }
    }

}
