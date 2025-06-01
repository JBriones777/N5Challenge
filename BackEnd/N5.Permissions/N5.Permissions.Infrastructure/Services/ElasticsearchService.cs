using Elastic.Clients.Elasticsearch;
using N5.Permissions.Application.Contracts;

namespace N5.Permissions.Infrastructure.Services;

public class ElasticsearchService(ElasticsearchClient client) : IElasticsearchService
{

    private readonly ElasticsearchClient _client = client;

    public async Task IndexDocumentAsync<T>(T document, string? indexName = null) where T : class
    {
        var response = await _client.IndexAsync(document);

        if (!response.IsValidResponse)
        {
            throw new InvalidOperationException($"Elasticsearch index error: {response.DebugInformation}");
        }
    }

}
