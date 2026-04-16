using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Store.Application.Abstractions.Interfaces.Commands;
using Store.Application.Commands;
using Store.Application.Common.Behaviors;
using System.Reflection;

namespace Store.Application.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddScoped<ValidationBehavior>();
        services.AddScoped<IAppDispatcher, AppDispatcher>();

        var handlerInterfaces = new[] { typeof(ICommandHandler<>), typeof(ICommandHandler<,>) };

        var handlers = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && handlerInterfaces.Contains(i.GetGenericTypeDefinition())));

        foreach (var handler in handlers)
        {
            var interfaces = handler.GetInterfaces()
                .Where(i => i.IsGenericType && handlerInterfaces.Contains(i.GetGenericTypeDefinition()));

            foreach (var @interface in interfaces)
            {
                services.AddScoped(@interface, handler);
            }
        }

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
