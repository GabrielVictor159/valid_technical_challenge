using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Api.DTOs.Requests.Order;
using Store.Application.Abstractions.DTOs.Responses.Common;
using Store.Application.Abstractions.DTOs.Responses.Order;
using Store.Application.Abstractions.Interfaces.Commands;
using Store.Application.Commands.Order.CreateOrder;
using Store.Application.Commands.Order.QueryOrder;

namespace Store.Api.Controllers;

[Authorize]
[Route("api/orders")]
public class OrderController : ApiControllerBase
{
    private readonly IAppDispatcher _dispatcher;

    public OrderController(IAppDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpPost]
    public async Task<ActionResult<CreateOrderResponse>> Create([FromBody] CreateOrderRequest request)
    {
        var command = request.Adapt<CreateOrderCommand>() with { User = CurrentUser };

        var result = await _dispatcher.Send(command);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<SearchOrderResponse>> GetById(long id)
    {
        var query = new GetOrderByIdQuery(id, CurrentUser);

        var result = await _dispatcher.Send(query);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<SearchOrderResponse>>> GetPaged([FromQuery] OrderSearchRequest request)
    {
        var query = request.Adapt<GetPagedOrdersQuery>() with { User = CurrentUser };

        var result = await _dispatcher.Send(query);
        return Ok(result);
    }

}
