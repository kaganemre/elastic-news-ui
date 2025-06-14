using Elastic.Clients.Elasticsearch;
using ElasticNews.Application.Services;
using ElasticNews.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticNews.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient<ICrawlerService, CrawlerService>(client =>
        {
            client.BaseAddress = new Uri("https://www.sozcu.com.tr/");
        });

        var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"));
        var client = new ElasticsearchClient(settings);
        services.AddSingleton(client);

        services.AddScoped<IElasticsearchService, ElasticsearchService>();

        return services;
    }

}