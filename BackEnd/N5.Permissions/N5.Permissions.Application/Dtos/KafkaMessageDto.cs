namespace N5.Permissions.Application.Dtos;

public class KafkaMessageDto
{
    public Guid Id { get; set; }
    public string NameOperation { get; set; } = default!;
}
