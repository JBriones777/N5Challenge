namespace N5.Permissions.Application.Contracts;

public interface IElasticsearchService
{
    Task IndexDocumentAsync<T>(T document, string? indexName = null) where T : class;
}