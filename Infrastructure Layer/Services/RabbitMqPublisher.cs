using Application_Layer.Interfaces;
using Domain_Layer.Entities;
using Infrastructure_Layer.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Infrastructure_Layer.Services;

/// <summary>
/// Service för att publicera meddelanden till RabbitMQ.
/// </summary>
public class RabbitMqPublisher : IMessagePublisher
{
    private readonly ILogger<RabbitMqPublisher> _logger;
    private readonly RabbitMqSettings _settings;

    // ActivitySource används för OpenTelemetry tracing
    private static readonly ActivitySource ActivitySource =
        new("Infrastructure_Layer.Services.RabbitMqPublisher");

    public RabbitMqPublisher(
        ILogger<RabbitMqPublisher> logger,
        IOptions<RabbitMqSettings> options)
    {
        _logger = logger;
        _settings = options.Value;
    }

    /// <summary>
    /// Publicerar filinformation till RabbitMQ queue.
    /// </summary>
    public async Task PublishFileMessageAsync(FileEntity file)
    {
        using var activity =
            ActivitySource.StartActivity("PublishFileMessage");

        // OpenTelemetry tags
        activity?.SetTag("messaging.system", "rabbitmq");
        activity?.SetTag("messaging.destination", _settings.QueueName);
        activity?.SetTag("file.name", file.FileName);

        try
        {
            // Skapar RabbitMQ connection factory
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            
            await using var connection =
                await factory.CreateConnectionAsync();

           
            await using var channel =
                await connection.CreateChannelAsync();

            // Skapar queue om den inte redan finns
            await channel.QueueDeclareAsync(
                queue: _settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Serialiserar filen till JSON
            var message = JsonSerializer.Serialize(file);

            // Konverterar JSON till byte array
            var body = Encoding.UTF8.GetBytes(message);

            // Publicerar meddelandet till RabbitMQ
            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: _settings.QueueName,
                body: body);

            activity?.SetStatus(ActivityStatusCode.Ok);

            _logger.LogInformation(
                "File message published successfully. FileName: {FileName}",
                file.FileName);
        }
        catch (Exception ex)
        {
            activity?.SetStatus(
                ActivityStatusCode.Error,
                ex.Message);

            _logger.LogError(
                ex,
                "Failed to publish file message. FileName: {FileName}",
                file.FileName);

            throw;
        }
    }
}