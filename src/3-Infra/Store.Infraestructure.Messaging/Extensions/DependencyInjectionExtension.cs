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
    public static IServiceCollection AddInfrastructureMessaging(
    this IServiceCollection services,
    IConfiguration configuration,
    bool isWorker = false)
    {
        var rabbitOptions = new RabbitMqOptions();
        configuration.GetSection(RabbitMqOptions.SectionName).Bind(rabbitOptions);

        services.AddRebus(configure => {
            var connectionString = $"amqp://{rabbitOptions.Username}:{rabbitOptions.Password}@{rabbitOptions.Host}";

            if (isWorker)
            {
                configure.Transport(t => t.UseRabbitMq(connectionString, "store.orders.queue"));
            }
            else
            {
                configure.Transport(t => t.UseRabbitMqAsOneWayClient(connectionString));
            }

            return configure
                .Routing(r => r.TypeBased().Map<CreateOrderEvent>("store.orders.queue"))
                .Options(o => {
                    o.SetMaxParallelism(5);
                    o.SetNumberOfWorkers(1);
                    o.RetryStrategy(maxDeliveryAttempts: 5, errorQueueName: "store.orders.error");
                });
        });

        return services;
    }
}
