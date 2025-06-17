using ElasticNews.Application.Services;
using ElasticNews.Domain.Entities;

namespace ElasticNews.Infrastructure.Services;

public sealed class NewsService(IElasticsearchService elasticsearchService) : INewsService
{
    public async Task<(List<News> Results, int TotalCount)> SearchAsync(string? query, int page, int pageSize)
    {
        return await elasticsearchService.SearchAsync(query, page, pageSize);
    }
}