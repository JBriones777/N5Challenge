using Microsoft.AspNetCore.Http;
using Moq;
using N5.Permissions.API.Filters;
using N5.Permissions.Application.Contracts;
using N5.Permissions.Application.Dtos;
using N5.Permissions.Application.UsesCases.PermissionOperations.Dtos;

namespace N5.Permissions.UnitTest;

public class OperationsFilterTests
{
    private readonly Mock<IKafkaProducerService> _kafkaMock = new();
    private readonly Mock<IElasticsearchService> _elasticMock = new();

    private OperationsFilter CreateSut() =>
        new OperationsFilter(_kafkaMock.Object, _elasticMock.Object);

    private static EndpointFilterInvocationContext CreateContext(object? argument, string httpMethod)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = httpMethod;
        var args = argument != null ? new List<object?> { argument } : new List<object?>();
        return new TestEndpointFilterInvocationContext(httpContext, args);
    }

    [Fact]
    public async Task Should_Index_GetPermissionByIdDto()
    {
        // Arrange
        var dto = new GetPermissionByIdDto();
        var context = CreateContext(dto, "GET");
        var sut = CreateSut();

        _elasticMock.Setup(e => e.IndexDocumentAsync(dto, null)).Returns(Task.CompletedTask).Verifiable();
        _kafkaMock.Setup(k => k.PublishAsync(It.IsAny<KafkaMessageDto>())).Returns(Task.CompletedTask);

        // Act
        await sut.InvokeAsync(context, _ => new ValueTask<object?>(Task.FromResult<object?>(null)));

        // Assert
        _elasticMock.Verify();
    }

    [Fact]
    public async Task Should_Index_PermissionDto()
    {
        // Arrange
        var dto = new PermissionDto();
        var context = CreateContext(dto, "POST");
        var sut = CreateSut();

        _elasticMock.Setup(e => e.IndexDocumentAsync(dto, null)).Returns(Task.CompletedTask).Verifiable();
        _kafkaMock.Setup(k => k.PublishAsync(It.IsAny<KafkaMessageDto>())).Returns(Task.CompletedTask);

        // Act
        await sut.InvokeAsync(context, _ => new ValueTask<object?>(Task.FromResult<object?>(null)));

        // Assert
        _elasticMock.Verify();
    }

    [Theory]
    [InlineData("GET", "get")]
    [InlineData("POST", "request")]
    [InlineData("PUT", "modify")]
    [InlineData("DELETE", "unknown")]
    public async Task Should_Publish_KafkaMessage_With_Correct_Operation(string httpMethod, string expectedName)
    {
        // Arrange
        var context = CreateContext(null, httpMethod);
        var sut = CreateSut();

        KafkaMessageDto? captured = null;
        _kafkaMock.Setup(k => k.PublishAsync(It.IsAny<KafkaMessageDto>()))
            .Callback<KafkaMessageDto>(msg => captured = msg)
            .Returns(Task.CompletedTask);

        // Act
        await sut.InvokeAsync(context, _ => new ValueTask<object?>(Task.FromResult<object?>(null)));

        // Assert
        Assert.NotNull(captured);
        Assert.Equal(expectedName, captured.NameOperation);
    }

    [Fact]
    public async Task Should_Call_Next_And_Return_Result()
    {
        // Arrange
        var context = CreateContext(null, "GET");
        var sut = CreateSut();
        var expected = new object();

        _kafkaMock.Setup(k => k.PublishAsync(It.IsAny<KafkaMessageDto>())).Returns(Task.CompletedTask);

        // Act
        var result = await sut.InvokeAsync(context, _ => new ValueTask<object?>(Task.FromResult<object?>(expected)));

        // Assert
        Assert.Same(expected, result);
    }

    private class TestEndpointFilterInvocationContext : EndpointFilterInvocationContext
    {
        public override HttpContext HttpContext { get; }
        public override IList<object?> Arguments { get; } 

        public TestEndpointFilterInvocationContext(HttpContext httpContext, IList<object?> args) 
        {
            HttpContext = httpContext;
            Arguments = args;
        }

        public override T GetArgument<T>(int index)
        {
            return (T)Arguments[index]!;
        }
    }
}
