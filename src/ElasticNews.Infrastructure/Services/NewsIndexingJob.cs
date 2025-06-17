using ElasticNews.Application.Services;

namespace ElasticNews.Infrastructure.Services;

public sealed class NewsIndexingJob(
    ICrawlerService crawlerService,
    IElasticsearchService elasticsearchService) : INewsIndexingJob
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
        }
    }
}