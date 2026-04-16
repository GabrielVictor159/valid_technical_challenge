using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Application.Abstractions.Interfaces.Repositories;
using Store.Infraestructure.Data.Contexts;
using Store.Infraestructure.Data.Repositories;

namespace Store.Infraestructure.Data.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfrastructureData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StoreContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        return services;
    }
}
