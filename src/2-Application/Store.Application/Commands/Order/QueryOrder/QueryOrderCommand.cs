using Store.Application.Abstractions.DTOs;
using Store.Application.Abstractions.DTOs.Responses.Common;
using Store.Application.Abstractions.DTOs.Responses.Order;
using Store.Application.Abstractions.Interfaces.Commands;

namespace Store.Application.Commands.Order.QueryOrder;

public record GetOrderByIdQuery(long Id, UserSession? User) : ICommand<SearchOrderResponse?>;

public record GetPagedOrdersQuery(int Page, int PageSize, string? SearchTerm, UserSession? User)
    : ICommand<PagedResponse<SearchOrderResponse>>;
