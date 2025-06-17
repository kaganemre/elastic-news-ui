using Elastic.Clients.Elasticsearch;
using ElasticNews.Application.Services;
using ElasticNews.Domain.Entities;

namespace ElasticNews.Infrastructure.Services;

public sealed class ElasticsearchService(ElasticsearchClient elasticClient) : IElasticsearchService
{
    private readonly ElasticsearchClient _elasticClient = elasticClient;
    public async Task IndexNewsAsync(IEnumerable<News> newsList)
    {
        foreach (var news in newsList)
        {
            var response = await _elasticClient.IndexAsync(news, idx => idx.Index("news"));

            if (!response.IsValidResponse)
            {
                Console.WriteLine($"Indexing failed for {news.Id}");
            }
        }
    }
    public async Task<(List<News> Results, int TotalCount)> SearchAsync(string? query, int page, int pageSize)
    {
        var from = (page - 1) * pageSize;

        var response = await _elasticClient.SearchAsync<News>(s => s
            .Indices("news")
            .From(from)
            .Size(pageSize)
            .Query(string.IsNullOrEmpty(query)
                  ? q => q.MatchAll()
                  : q => q.QueryString(qs => qs.Query(query))
            )
        );

        return (
            Results: response.Documents.ToList(),
            TotalCount: (int)response.Total
        );
    }
    public async Task<List<News>> GetAllNewsAsync()
    {
        var response = await _elasticClient.SearchAsync<News>(s => s
            .Indices("news")
            .Query(q => q.MatchAll()));

        return response.Documents.ToList();
    }
}
