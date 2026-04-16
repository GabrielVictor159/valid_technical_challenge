using Mapster;
using Microsoft.Extensions.Logging;
using Store.Application.Abstractions.DTOs.Responses.Common;
using Store.Application.Abstractions.DTOs.Responses.Order;
using Store.Application.Abstractions.Interfaces.Commands;
using Store.Application.Abstractions.Interfaces.Repositories;

namespace Store.Application.Commands.Order.QueryOrder;

public class QueryOrderCommandHandler :
    ICommandHandler<GetOrderByIdQuery, SearchOrderResponse?>,
    ICommandHandler<GetPagedOrdersQuery, PagedResponse<SearchOrderResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<QueryOrderCommandHandler> _logger;

    public QueryOrderCommandHandler(IUnitOfWork uow, ILogger<QueryOrderCommandHandler> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    public async Task<SearchOrderResponse?> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
        _logger.LogInformation("Usuário {UserId} ({Email}) consultando pedido ID {OrderId}",
            request.User?.UserId, request.User?.Email, request.Id);

        var order = (await _uow.Repository<Store.Domain.Entities.Order>().FindAsync(e=>e.Id==request.Id,e=>e.Operations)).FirstOrDefault();

        if (order == null)
        {
            _logger.LogWarning("Pedido {OrderId} não encontrado para o usuário {UserId}", request.Id, request.User?.UserId);
        }

        return order?.Adapt<SearchOrderResponse>();
    }

    public async Task<PagedResponse<SearchOrderResponse>> Handle(GetPagedOrdersQuery request, CancellationToken ct)
    {
        _logger.LogInformation("Usuário {UserId} realizando busca paginada de pedidos. Página: {Page}, Termo: {Term}",
            request.User?.UserId, request.Page, request.SearchTerm ?? "N/A");

        var (items, total) = await _uow.Repository<Store.Domain.Entities.Order>().FindPagedAsync(
            request.Page,
            request.PageSize,
            x => string.IsNullOrEmpty(request.SearchTerm) || x.NumberOrder.Contains(request.SearchTerm)
        );

        var dtos = items.Adapt<IEnumerable<SearchOrderResponse>>();

        _logger.LogInformation("Busca concluída para usuário {UserId}. Total de registros: {Total}",
            request.User?.UserId, total);

        return new PagedResponse<SearchOrderResponse>(dtos, total, request.Page, request.PageSize);
    }
}