using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Store.Api.Controllers;
using Store.Api.DTOs.Requests.Order;
using Store.Application.Abstractions.DTOs.Responses.Common;
using Store.Application.Abstractions.DTOs.Responses.Order;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Infraestructure.Data.Contexts;

namespace Store.Tests.Tests;

public class OrderQueryTests : IntegrationTestBase
{
    private async Task SeedDatabaseAsync(StoreContext context)
    {
        var faker = new Bogus.Faker();

        var operationFaker = new Bogus.Faker<Operation>()
            .RuleFor(o => o.Status, f => f.PickRandom<OrderStatusEnum>())
            .RuleFor(o => o.Message, f => f.Lorem.Sentence());

        var orderFaker = new Bogus.Faker<Order>()
            .RuleFor(o => o.TotalPrice, f => f.Finance.Amount(50, 500))
            .RuleFor(o => o.Status, f => f.PickRandom<OrderStatusEnum>())
            .RuleFor(o => o.Operations, (f, o) => operationFaker.Generate(f.Random.Number(1, 2)));

        var orders = new List<Order>();

        orders.AddRange(orderFaker
            .RuleFor(o => o.NumberOrder, f => $"ABC-{f.Random.AlphaNumeric(5).ToUpper()}")
            .Generate(2));

        orders.AddRange(orderFaker
            .RuleFor(o => o.NumberOrder, f => $"XYZ-{f.Random.AlphaNumeric(5).ToUpper()}")
            .Generate(1));

        context.Orders.AddRange(orders);
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetById_ShouldReturnOrder_WhenExists()
    {
        using var scope = ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
        await SeedDatabaseAsync(context);

        var seededOrder = await context.Orders.Include(x => x.Operations).FirstAsync();
        var controller = GetController<OrderController>();

        var actionResult = await controller.GetById(seededOrder.Id);

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var response = Assert.IsType<SearchOrderResponse>(okResult.Value);

        Assert.Equal(seededOrder.NumberOrder, response.NumberOrder);
        Assert.Equal(seededOrder.Status, response.Status);
        Assert.Equal(seededOrder.Operations.Count, response.Operations.Count());
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        var controller = GetController<OrderController>();

        var actionResult = await controller.GetById(9999);

        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public async Task GetPaged_ShouldReturnFilteredResults_WhenSearchTermIsProvided()
    {
        using var scope = ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
        await SeedDatabaseAsync(context);

        var controller = GetController<OrderController>();
        var searchRequest = new OrderSearchRequest
        {
            Page = 1,
            PageSize = 10,
            SearchTerm = "ABC"
        };

        var actionResult = await controller.GetPaged(searchRequest);

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var response = Assert.IsType<PagedResponse<SearchOrderResponse>>(okResult.Value);

        // O seed gerou exatamente 2 pedidos com ABC
        Assert.Equal(2, response.TotalCount);
        Assert.All(response.Items, x => Assert.Contains("ABC", x.NumberOrder));
    }

    [Fact]
    public async Task GetPaged_ShouldReturnAllResults_WhenNoSearchTerm()
    {
        using var scope = ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
        await SeedDatabaseAsync(context);

        var controller = GetController<OrderController>();
        var searchRequest = new OrderSearchRequest { Page = 1, PageSize = 10 };

        var actionResult = await controller.GetPaged(searchRequest);

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var response = Assert.IsType<PagedResponse<SearchOrderResponse>>(okResult.Value);

        // Total de 3 pedidos gerados no seed
        Assert.Equal(3, response.TotalCount);
    }
}