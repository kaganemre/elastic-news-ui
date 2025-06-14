using System.Text.RegularExpressions;
using ElasticNews.Application.Services;
using ElasticNews.Domain.Entities;
using HtmlAgilityPack;

namespace ElasticNews.Infrastructure.Services;

public sealed class CrawlerService(HttpClient httpClient) : ICrawlerService
{
    private readonly HttpClient _httpClient = httpClient;
    public async Task<IEnumerable<News>> CrawlNewsAsync()
    {
        var html = await _httpClient.GetStringAsync("/");

        var document = new HtmlDocument();
        document.LoadHtml(html);

        var newsCards = document.DocumentNode.SelectNodes("//div[@class='news-card']");

        if (newsCards is not null)
        {
            var news = newsCards.Select(card =>
            {
                var titleNode = card.SelectSingleNode(".//a[contains(@class, 'news-card-footer')]");
                var imageNode = card.SelectSingleNode(".//img");
                var url = "https://www.sozcu.com.tr" + titleNode?.GetAttributeValue("href", "");

                var match = Regex.Match(url, @"p\d+$");

                return new News
                {
                    Id = match.Success ? match.Value : "",
                    Title = titleNode?.InnerText.Trim() ?? "",
                    Url = url,
                    ImageUrl = imageNode?.GetAttributeValue("src", "") ?? ""
                };
            }).ToList();

            return news;
        }

        return new List<News>();
    }
}