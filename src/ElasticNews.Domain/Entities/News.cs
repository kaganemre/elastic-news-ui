namespace ElasticNews.Domain.Entities;

public sealed class News
{
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
}