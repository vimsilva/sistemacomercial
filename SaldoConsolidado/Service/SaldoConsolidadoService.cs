using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using SaldoConsolidado.Domain;
using SaldoConsolidado.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SaldoConsolidado.Infrastructure.RabbitMQ;

namespace SaldoConsolidado.Service;
public class SaldoConsolidadoService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly RabbitMQSettings _rabbitMQSettings;
    private IConnection _connection;
    private IModel _channel;

    public SaldoConsolidadoService(IServiceScopeFactory scopeFactory, IOptions<RabbitMQSettings> rabbitMQSettings)
    {
        _scopeFactory = scopeFactory;
        _rabbitMQSettings = rabbitMQSettings.Value;
        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMQSettings.HostName,
            UserName = _rabbitMQSettings.UserName,
            Password = _rabbitMQSettings.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _rabbitMQSettings.QueueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.Register(() =>
        {
            _channel?.Close();
            _connection?.Close();
        });

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = System.Text.Encoding.UTF8.GetString(body);
            var lancamento = JsonSerializer.Deserialize<Lancamento>(message);

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SaldoConsolidadoContext>();

            var saldoConsolidado = await context.SaldosConsolidados
                .FirstOrDefaultAsync(s => s.Data.Date == lancamento.DataLancamento.Date);

            if (saldoConsolidado == null)
            {
                saldoConsolidado = new SaldoConsolidadoEntity
                {
                    Id = Guid.NewGuid(),
                    Data = lancamento.DataLancamento.Date,
                    Saldo = 0
                };
                context.SaldosConsolidados.Add(saldoConsolidado);
            }

            saldoConsolidado.Saldo += lancamento.Tipo == "debito" ? -lancamento.Valor : lancamento.Valor;

            await context.SaveChangesAsync();
        };

        _channel.BasicConsume(queue: _rabbitMQSettings.QueueName,
                             autoAck: true,
                             consumer: consumer);

        return Task.CompletedTask;
    }
}
