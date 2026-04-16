using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Rebus.Bus;
using Store.Api.Controllers;
using Store.Application.Abstractions.Interfaces.Repositories;
using Store.Application.Extensions;
using Store.Domain.Enums;
using Store.Infraestructure.Data.Contexts;
using Store.Infraestructure.Data.Repositories;
using Store.Worker.Consumers;
using System.Security.Claims;

namespace Store.Tests.Tests;

public abstract class IntegrationTestBase : IDisposable
{
    protected readonly IServiceProvider ServiceProvider;
    protected readonly SqliteConnection Connection;
    protected readonly IBus BusMock;

    protected IntegrationTestBase()
    {
        var services = new ServiceCollection();

        Connection = new SqliteConnection("DataSource=:memory:");
        Connection.Open();

        services.AddDbContext<StoreContext>(options => options.UseSqlite(Connection));
        services.AddLogging();

        services.AddApplication();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<OrderController>();

        services.AddTransient<OrderCreatedConsumer>();

        BusMock = Substitute.For<IBus>();
        services.Replace(ServiceDescriptor.Singleton(BusMock));

        ServiceProvider = services.BuildServiceProvider();

        using var scope = ServiceProvider.CreateScope();
        scope.ServiceProvider.GetRequiredService<StoreContext>().Database.EnsureCreated();
    }

    protected T GetController<T>(string userId = "1", string email = "test@store.com", string name = "Admin") where T : ControllerBase
    {
        var controller = ServiceProvider.GetRequiredService<T>();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, name)
        };

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth")) }
        };

        return controller;
    }

    public void Dispose()
    {
        Connection.Close();
        Connection.Dispose();
    }
}