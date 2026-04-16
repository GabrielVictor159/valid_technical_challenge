using Store.Application.Extensions;
using Store.Infraestructure.Messaging.Extensions;
using Store.Infraestructure.Data.Extensions;
using System.Reflection;
using Store.Worker.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructureData(builder.Configuration);
builder.Services.AddConsumers(builder.Configuration);
builder.Services.AddInfrastructureMessaging(builder.Configuration);
builder.Services.AddApplication();

var host = builder.Build();
host.Run();