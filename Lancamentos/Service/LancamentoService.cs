using Lancamentos.Domain.Repository;
using Lancamentos.Domain;
using Lancamentos.Domain.Service;
using Lancamentos.Infrastruture.RabbitMQ;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;

namespace Lancamentos.Service;

public class LancamentoService : ILancamentoService
{
    private readonly ILancamentoRepository _lancamentoRepository;
    private readonly RabbitMQSettings _rabbitMQSettings;

    public LancamentoService(ILancamentoRepository lancamentoRepository, IOptions<RabbitMQSettings> rabbitMQSettings)
    {
        _lancamentoRepository = lancamentoRepository;
        _rabbitMQSettings = rabbitMQSettings.Value;
    }
    public async Task<Lancamento> Create(Lancamento lancamento)
    {
        Validate(lancamento);
        lancamento.Id = Guid.NewGuid();
        lancamento.DataCriacao = DateTime.UtcNow;
        lancamento.DataAtualizacao = DateTime.UtcNow;
        await _lancamentoRepository.Save(lancamento);
        PublishToRabbitMQ(lancamento);
        return lancamento;
    }

    public async Task Delete(Guid id)
    {
        await _lancamentoRepository.Delete(id);
    }

    public async Task<List<Lancamento>> GetAll()
    {
        return await _lancamentoRepository.FindAll();
    }

    public async Task<Lancamento> GetById(Guid id)
    {
        return await _lancamentoRepository.FindById(id);
    }

    public async Task<Lancamento> Update(Lancamento lancamento)
    {
        Validate(lancamento);
        lancamento.DataAtualizacao = DateTime.UtcNow;
        await _lancamentoRepository.Update(lancamento);
        return lancamento;
    }

    public void Validate(Lancamento lancamento)
    {
        if (lancamento == null)
            throw new ArgumentNullException(nameof(lancamento));

        if (string.IsNullOrEmpty(lancamento.Tipo) || (lancamento.Tipo != "debito" && lancamento.Tipo != "credito"))
            throw new ArgumentException("Tipo de lançamento inválido. Deve ser 'debito' ou 'credito'.");

        if (lancamento.Valor <= 0)
            throw new ArgumentException("Valor do lançamento deve ser positivo.");

        if (lancamento.DataLancamento == default)
            throw new ArgumentException("Data de lançamento inválida.");
    }

    private void PublishToRabbitMQ(Lancamento lancamento)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMQSettings.HostName,
            UserName = _rabbitMQSettings.UserName,
            Password = _rabbitMQSettings.Password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: _rabbitMQSettings.QueueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var message = JsonSerializer.Serialize(lancamento);
        var body = System.Text.Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
                             routingKey: _rabbitMQSettings.QueueName,
                             basicProperties: null,
                             body: body);
    }
}
