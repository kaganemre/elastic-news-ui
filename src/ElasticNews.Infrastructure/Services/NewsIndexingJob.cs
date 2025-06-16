using System.Text.Json;
using ElasticNews.Application.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace ElasticNews.Infrastructure.Services;

public sealed class NewsIndexingJob(
    ICrawlerService crawlerService,
    IElasticsearchService elasticsearchService,
    IDistributedCache cache) : INewsIndexingJob
{
    public async Task FetchAndIndexNewsAsync()
    {
        var crawledNews = await crawlerService.CrawlNewsAsync();
        var indexedNews = await elasticsearchService.GetAllNewsAsync();

        var indexedNewsIds = indexedNews.Select(n => n.Id).ToHashSet();

        var newsToIndex = crawledNews
            .Where(n => !indexedNewsIds.Contains(n.Id))
            .ToList();

        if (newsToIndex.Any())
        {
            await elasticsearchService.IndexNewsAsync(newsToIndex);

            indexedNews = await elasticsearchService.GetAllNewsAsync();
            await cache.SetStringAsync("allNews", JsonSerializer.Serialize(indexedNews));
        }
    }
}