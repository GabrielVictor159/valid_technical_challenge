using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Store.Api.Controllers;
using Store.Api.DTOs.Requests.Order;
using Store.Application.Abstractions.DTOs.Responses.Order;
using Store.Domain.Contracts.Order;
using Store.Domain.Enums;
using Store.Infraestructure.Data.Contexts;
using Store.Worker.Consumers;
using Bogus;

namespace Store.Tests.Tests;

public class OrderIntegrationTests : IntegrationTestBase
{
    private readonly Faker<CreateOrderRequest> _orderRequestFaker;

    public OrderIntegrationTests()
    {
        _orderRequestFaker = new Faker<CreateOrderRequest>()
            .CustomInstantiator(f => new CreateOrderRequest(
                f.Commerce.Ean13(),
                f.Finance.Amount(10, 1000)
            ));
    }

    [Fact]
    public async Task CreateOrder_ShouldFlowFromControllerToWorker()
    {
        var controller = GetController<OrderController>();
        var request = _orderRequestFaker.Generate();

        var actionResult = await controller.Create(request);
        var response = Assert.IsType<CreateOrderResponse>((actionResult.Result as OkObjectResult)!.Value);

        var eventMessage = BusMock.ReceivedCalls()
            .Select(c => c.GetArguments()[0])
            .OfType<CreateOrderEvent>()
            .FirstOrDefault(x => x.OrderId == response.Id);

        Assert.NotNull(eventMessage);

        using (var scope = ServiceProvider.CreateScope())
        {
            var consumer = scope.ServiceProvider.GetRequiredService<OrderCreatedConsumer>();
            await consumer.Handle(eventMessage);
        }

        using (var finalScope = ServiceProvider.CreateScope())
        {
            var context = finalScope.ServiceProvider.GetRequiredService<StoreContext>();
            var orderProcessed = await context.Orders
                .Include(x => x.Operations)
                .FirstOrDefaultAsync(x => x.Id == response.Id);

            Assert.Equal(OrderStatusEnum.PROCESSADO, orderProcessed!.Status);
            Assert.Equal(request.TotalPrice * 1.27m, orderProcessed.TotalPrice);
        }
    }

    [Fact]
    public async Task CreateOrder_ShouldFlowFromControllerToWorker_WithCompleteHistory()
    {
        var controller = GetController<OrderController>();
        var request = _orderRequestFaker.Generate();

        var actionResult = await controller.Create(request);
        var response = Assert.IsType<CreateOrderResponse>((actionResult.Result as OkObjectResult)!.Value);

        var eventMessage = BusMock.ReceivedCalls()
            .Select(c => c.GetArguments()[0])
            .OfType<CreateOrderEvent>()
            .FirstOrDefault(x => x.OrderId == response.Id);

        using (var scope = ServiceProvider.CreateScope())
        {
            var consumer = scope.ServiceProvider.GetRequiredService<OrderCreatedConsumer>();
            await consumer.Handle(eventMessage!);
        }

        using (var finalScope = ServiceProvider.CreateScope())
        {
            var context = finalScope.ServiceProvider.GetRequiredService<StoreContext>();
            var order = await context.Orders
                .Include(x => x.Operations)
                .FirstOrDefaultAsync(x => x.Id == response.Id);

            Assert.Equal(3, order!.Operations.Count);
            var operations = order.Operations.OrderBy(x => x.CreatedDate).ToList();
            Assert.Equal(OrderStatusEnum.RECEBIDO, operations[0].Status);
            Assert.Equal(OrderStatusEnum.EM_PROCESSAMENTO, operations[1].Status);
            Assert.Equal(OrderStatusEnum.PROCESSADO, operations[2].Status);
        }
    }
}