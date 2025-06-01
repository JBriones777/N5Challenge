using N5.Permissions.Application.Dtos;

namespace N5.Permissions.Application.Contracts;

public interface IKafkaProducerService
{
    Task PublishAsync(KafkaMessageDto message);
}