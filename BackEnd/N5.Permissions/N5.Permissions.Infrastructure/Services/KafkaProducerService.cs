using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using N5.Permissions.Application.Contracts;
using N5.Permissions.Application.Dtos;
using System.Text.Json;

namespace N5.Permissions.Infrastructure.Services;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;

    public KafkaProducerService(IProducer<Null, string> producer, IConfiguration configuration)
    {
        _producer = producer;
        _topic = configuration["Kafka:Topic"]!;
    }

    public async Task PublishAsync(KafkaMessageDto message)
    {
        var payload = JsonSerializer.Serialize(message);

        await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = payload });
    }
}
