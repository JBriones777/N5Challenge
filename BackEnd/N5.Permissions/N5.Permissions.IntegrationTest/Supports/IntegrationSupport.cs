using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using N5.Permissions.Application.Contracts;
using N5.Permissions.Application.Dtos;
using N5.Permissions.Infrastructure.Persistence;

namespace N5.Permissions.IntegrationTest.Supports;
public class IntegrationSupport : IDisposable
{
    public WebApplicationFactory<Program> Factory { get; }
    public HttpClient Client { get; }

    public IntegrationSupport()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTest");
        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(ConfigureTestServices);
            });

        Client = Factory.CreateClient();
    }

    protected virtual void ConfigureTestServices(IServiceCollection services)
    {
        var kafkaDescriptor = services.SingleOrDefault(
        d => d.ServiceType == typeof(IKafkaProducerService));
        if (kafkaDescriptor != null)
            services.Remove(kafkaDescriptor);

        var elasticDescriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(IElasticsearchService));
        if (elasticDescriptor != null)
            services.Remove(elasticDescriptor);

        var kafkaMock = new Mock<IKafkaProducerService>();
        kafkaMock
            .Setup(x => x.PublishAsync(It.IsAny<KafkaMessageDto>()))
            .Returns(Task.CompletedTask);

        services.AddSingleton(kafkaMock.Object);

        var elasticMock = new Mock<IElasticsearchService>();
        elasticMock
            .Setup(x => x.IndexDocumentAsync<object>(It.IsAny<object>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        services.AddSingleton(elasticMock.Object);
    }
    public void Dispose()
    {
        Client.Dispose();
        Factory.Dispose();
    }
}
