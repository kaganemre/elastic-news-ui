using ElasticNews.Domain.Entities;

namespace ElasticNews.Application.Services;

public interface IElasticsearchService
{
    Task IndexNewsAsync(IEnumerable<News> newsList);
    public Task<(List<News> Results, int TotalCount)> SearchAsync(string? query, int page, int pageSize);
    Task<List<News>> GetAllNewsAsync();
}