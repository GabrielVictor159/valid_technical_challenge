using Rebus.Bus;
using Rebus.Handlers;
using Store.Application.Abstractions.Interfaces.Commands;
using Store.Application.Commands.Order.ProcessOrder;
using Store.Domain.Contracts.Order;

namespace Store.Worker.Consumers;
public class OrderCreatedConsumer : IHandleMessages<CreateOrderEvent>
{
    private readonly IBus _bus;
    private readonly IAppDispatcher _dispatcher;
    private readonly ILogger<OrderCreatedConsumer> _logger;

    public OrderCreatedConsumer(IBus bus, IAppDispatcher dispatcher, ILogger<OrderCreatedConsumer> logger)
    {
        _bus = bus;
        _dispatcher = dispatcher;
        _logger = logger;
    }

    public async Task Handle(CreateOrderEvent message)
    {
        _logger.LogInformation("Mensagem recebida do RabbitMQ via Rebus: {OrderId}", message.OrderId);
        try
        {
            await _dispatcher.Send(new ProcessOrderCommand(message.OrderId));
        }
        catch (Exception)
        {
            await _bus.Defer(TimeSpan.FromMinutes(1), message);
        }
    }
}