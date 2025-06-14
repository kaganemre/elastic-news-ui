using ElasticNews.Domain.Entities;

namespace ElasticNews.Application.Services;

public interface IElasticsearchService
{
    Task IndexNewsAsync(IEnumerable<News> newsList);
    Task<IEnumerable<News>> GetAllNewsAsync();
    Task<IEnumerable<News>> SearchByTitleAsync(string query);
}