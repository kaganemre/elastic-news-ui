namespace ElasticNews.Application.Services;

public interface INewsIndexingJob
{
    Task FetchAndIndexNewsAsync();
}