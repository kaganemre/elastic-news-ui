using System.Text.Json;
using ElasticNews.Application.Services;
using ElasticNews.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace ElasticNews.Infrastructure.Services;

public sealed class NewsService(
    IElasticsearchService elasticsearchService,
    IDistributedCache cache) : INewsService
{
    public async Task<IEnumerable<News>> GetAllNewsAsync()
    {
        var cachedNews = await cache.GetStringAsync("allNews") ?? "";
        var newsList = JsonSerializer.Deserialize<IEnumerable<News>>(cachedNews) ?? [];
        return newsList;
    }
    public async Task<IEnumerable<News>> SearchByTitleAsync(string query)
    {
        return await elasticsearchService.SearchByTitleAsync(query);
    }
}