using Elastic.Clients.Elasticsearch;
using ElasticNews.Application.Services;
using ElasticNews.Domain.Entities;

namespace ElasticNews.Infrastructure.Services;

public sealed class ElasticsearchService(ElasticsearchClient client) : IElasticsearchService
{
    private readonly ElasticsearchClient _client = client;
    public async Task IndexNewsAsync(IEnumerable<News> newsList)
    {
        foreach (var news in newsList)
        {
            var response = await _client.IndexAsync(news, idx => idx.Index("news"));

            if (!response.IsValidResponse)
            {
                Console.WriteLine($"Indexing failed for {news.Id}");
            }
        }
    }
    public async Task<IEnumerable<News>> GetAllNewsAsync()
    {
        var response = await _client.SearchAsync<News>(s => s
            .Indices("news")
            .Query(q => q.MatchAll()));

        return response.Documents;
    }
    public async Task<IEnumerable<News>> SearchByTitleAsync(string query)
    {
        var response = await _client.SearchAsync<News>(s => s
            .Indices("news")
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Title)
                    .Query(query)
                )
            )
        );

        return response.Documents;
    }
}