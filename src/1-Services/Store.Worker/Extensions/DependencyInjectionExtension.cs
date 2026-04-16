using Rebus.Config;
using Rebus.Handlers;
using Store.Domain.Contracts.Order;
using Store.Worker.Consumers;

namespace Store.Worker.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddConsumers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IHandleMessages<CreateOrderEvent>, OrderCreatedConsumer>();
        services.AutoRegisterHandlersFromAssemblyOf<OrderCreatedConsumer>();

        return services;
    }
}
