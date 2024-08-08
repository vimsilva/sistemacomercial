using Lancamentos.Domain;
using Lancamentos.Domain.Repository;
using Lancamentos.Domain.Service;
using Lancamentos.Infrastruture;
using Lancamentos.Infrastruture.RabbitMQ;
using Lancamentos.Infrastruture.Repository;
using Lancamentos.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lancamentos;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<ILancamentoRepository, LancamentoRepository>();
        builder.Services.AddScoped<ILancamentoService, LancamentoService>();
        builder.Services.AddScoped<ISaldoConsolidadoRepository, SaldoConsolidadoRepository>();
        builder.Services.AddScoped<ISaldoConsolidadoService, SaldoConsolidadoService>();

        builder.Services.AddDbContext<LancamentoContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddDbContext<SaldoConsolidadoContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQSettings"));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        //---------- API ------------

        app.MapPost("/api/lancamentos", async (Lancamento lancamento, ILancamentoService lancamentoService) =>
        {
            await lancamentoService.Create(lancamento);
            return Results.Created($"/api/lancamentos/{lancamento.Id}", lancamento);
        })
        .WithName("NovoLancamento")
        .WithOpenApi();

        app.MapGet("/api/lancamentos", async (ILancamentoService lancamentoService) =>
        {
            var lancamentos = await lancamentoService.GetAll();
            return Results.Ok(lancamentos);
        })
        .WithName("BuscaTodosLancamentos")
        .WithOpenApi();

        app.MapGet("/api/saldoconsolidado", async (ISaldoConsolidadoService saldoConsolidadoService) =>
        {
            var saldoconsolidado = await saldoConsolidadoService.GetSaldoConsolidado();
            return Results.Ok(saldoconsolidado);
        })
       .WithName("BuscaSaldoConsolidado")
       .WithOpenApi();


        //---------- FIM API ------------
        app.Run();
    }
}