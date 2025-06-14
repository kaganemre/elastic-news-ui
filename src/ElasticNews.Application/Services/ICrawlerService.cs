using ElasticNews.Domain.Entities;

namespace ElasticNews.Application.Services;

public interface ICrawlerService
{
    Task<IEnumerable<News>> CrawlNewsAsync();
}