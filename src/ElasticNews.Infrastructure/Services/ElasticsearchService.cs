using System.Net;
using System.Text;
using Elastic.Clients.Elasticsearch;
using ElasticNews.Application.Services;
using ElasticNews.Domain.Entities;
using Newtonsoft.Json;

namespace ElasticNews.Infrastructure.Services;

public sealed class ElasticsearchService(ElasticsearchClient client,
    HttpClient httpClient) : IElasticsearchService
{
    private readonly ElasticsearchClient _client = client;
    public async Task IndexNewsAsync(IEnumerable<News> newsList)
    {
        foreach (var news in newsList)
        {
            news.Title = WebUtility.HtmlDecode(news.Title);
            // var response = await _client.IndexAsync(news, idx => idx.Index("news"));
            var json = JsonConvert.SerializeObject(news, new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.Default
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.ContentType.CharSet = "utf-8";

            var response = await httpClient.PostAsync("http://localhost:9200/news/_doc", content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Indexing failed for {news.Id} - Status: {response.StatusCode}");
            }
        }
    }
    public async Task<IEnumerable<News>> GetAllNewsAsync()
    {
        var response = await _client.SearchAsync<News>(s => s
            .Indices("news")
            .Query(q => q.MatchAll()));

        return response.Documents;
    }
    public async Task<IEnumerable<News>> SearchByTitleAsync(string query)
    {
        var response = await _client.SearchAsync<News>(s => s
            .Indices("news")
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Title)
                    .Query(query)
                )
            )
        );

        return response.Documents;
    }
}