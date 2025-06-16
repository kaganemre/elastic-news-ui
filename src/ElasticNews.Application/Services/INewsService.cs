using ElasticNews.Domain.Entities;

namespace ElasticNews.Application.Services;

public interface INewsService
{
    Task<IEnumerable<News>> GetAllNewsAsync();
    Task<IEnumerable<News>> SearchByTitleAsync(string query);
}