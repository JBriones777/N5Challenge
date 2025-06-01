using Confluent.Kafka;
using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N5.Permissions.Application.Contracts;
using N5.Permissions.Domain.Contracts;
using N5.Permissions.Infrastructure.Persistence;
using N5.Permissions.Infrastructure.Repositories;
using N5.Permissions.Infrastructure.Services;

namespace N5.Permissions.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (environment == "IntegrationTest")
        {
            services.AddDbContext<PermissionsDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));

            services.AddDbContext<IReadOnlyDbContext, PermissionsDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
        }
        else
        {
            services.AddDbContext<PermissionsDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<IReadOnlyDbContext, PermissionsDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ReadOnlyConnection")));
        }

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton(sp =>
        {
            var uri = new Uri(configuration["Elasticsearch:Uri"]!);
            var defaultIndex = configuration["Elasticsearch:IndexName"]!;

            var settings = new ElasticsearchClientSettings(uri)
                .DefaultIndex(defaultIndex)
                .PrettyJson()
                .EnableDebugMode();

            return new ElasticsearchClient(settings);
        });

        services.AddSingleton(sp =>
        {
            return new ProducerBuilder<Null, string>(new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            }).Build();
        });

        services.AddScoped<IElasticsearchService, ElasticsearchService>();
        services.AddScoped<IKafkaProducerService, KafkaProducerService>();

        return services;
    }

    public static void CreateDatabase(this IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PermissionsDbContext>();
        dbContext.Database.EnsureCreated();
    }
}
