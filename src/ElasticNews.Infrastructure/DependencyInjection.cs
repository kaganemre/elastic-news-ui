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

        return services;
    }

}