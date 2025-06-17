using ElasticNews.Domain.Entities;

namespace ElasticNews.Application.Services;

public interface INewsService
{
    Task<(List<News> Results, int TotalCount)> SearchAsync(string? query, int page, int pageSize);
}