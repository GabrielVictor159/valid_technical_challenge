using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;
using Store.Domain.Contracts.Order;
using Store.Infraestructure.Messaging.Configuration;


namespace Store.Infraestructure.Messaging.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfrastructureMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitOptions = new RabbitMqOptions();
        configuration.GetSection(RabbitMqOptions.SectionName).Bind(rabbitOptions);

        services.AddRebus(configure => configure
            .Transport(t => t.UseRabbitMq(
                $"amqp://{rabbitOptions.Username}:{rabbitOptions.Password}@{rabbitOptions.Host}",
                "store.orders.queue"))
            .Routing(r => r.TypeBased().Map<CreateOrderEvent>("store.orders.queue"))
            .Options(o => {
                o.RetryStrategy(maxDeliveryAttempts: 5, errorQueueName: "store.orders.error");
            })
        );

        return services;
    }
}