using N5.Permissions.Application.Contracts;
using N5.Permissions.Application.Dtos;
using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;

namespace N5.Permissions.API.Filters;

public class OperationsFilter : IEndpointFilter
{
    private readonly IKafkaProducerService _kafkaProducerService;
    private readonly IElasticsearchService _elasticsearchService;

    public OperationsFilter(
        IKafkaProducerService kafkaProducerService,
        IElasticsearchService elasticsearchService)
    {
        _kafkaProducerService = kafkaProducerService;
        _elasticsearchService = elasticsearchService;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var arguments= context.Arguments.Count > 0 ? context.Arguments[0] : null;
        switch (arguments)
        {
            case GetPermissionByIdDto getByIdDto:
                await _elasticsearchService.IndexDocumentAsync(getByIdDto);
                break;
            case PermissionDto permissionDto:
                await _elasticsearchService.IndexDocumentAsync(permissionDto);
                break;
        }

        var operationName = context.HttpContext.Request.Method switch
        {
            "GET" => "get",
            "POST" => "request",
            "PUT" => "modify",
            _ => "unknown"
        };

        var kafkaMessage = new KafkaMessageDto
        {
            Id = Guid.NewGuid(),
            NameOperation = operationName
        };
        await _kafkaProducerService.PublishAsync(kafkaMessage);

        var result = await next(context);
        return result;
    }
}