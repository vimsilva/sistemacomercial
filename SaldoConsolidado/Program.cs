using SaldoConsolidado.Infrastructure;
using SaldoConsolidado.Infrastructure.RabbitMQ;
using SaldoConsolidado.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SaldoConsolidado;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<SaldoConsolidadoContext>(options =>
                    options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection")));

                services.Configure<RabbitMQSettings>(context.Configuration.GetSection("RabbitMQSettings"));

                services.AddHostedService<SaldoConsolidadoService>();
            });
}
