using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Store.Infraestructure.Data.Contexts;

namespace Store.Infraestructure.Data.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using StoreContext context = scope.ServiceProvider.GetRequiredService<StoreContext>();

        int retries = 10;
        int delayPerRetryInSeconds = 2;

        for (int i = 1; i <= retries; i++)
        {
            try
            {
                Console.WriteLine($"[Tentativa {i}/{retries}] Iniciando migration...");
                context.Database.Migrate();
                Console.WriteLine("Migrations aplicadas com sucesso!");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na tentativa {i}: O banco ainda não está pronto ({ex.Message})");

                if (i == retries)
                {
                    Console.WriteLine("Número máximo de tentativas atingido. Encerrando aplicação.");
                    throw;
                }

                Console.WriteLine($"Aguardando {delayPerRetryInSeconds} segundos para tentar novamente...");
                Thread.Sleep(delayPerRetryInSeconds * 1000);
            }
        }
    }
}
