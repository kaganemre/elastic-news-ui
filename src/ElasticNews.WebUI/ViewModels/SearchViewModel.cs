using ElasticNews.Domain.Entities;

namespace ElasticNews.WebUI.ViewModels;

public sealed class SearchViewModel
{
    public string? Query { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public List<News> Results { get; set; } = new();
    public int TotalResults { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalResults / PageSize);
}