using System.Net;
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

        var newsList = new List<News>();

        if (newsCards is not null)
        {
            foreach (var card in newsCards)
            {
                var titleNode = card.SelectSingleNode(".//a[contains(@class, 'news-card-footer')]");
                var href = titleNode?.GetAttributeValue("href", "");
                var imageNode = card.SelectSingleNode(".//img");
                var url = "https://www.sozcu.com.tr" + href;
                var match = Regex.Match(url, @"p\d+$");

                var content = await GetNewsContentAsync(href);

                var newsItem = new News
                {
                    Id = match.Success ? match.Value : "",
                    Title = WebUtility.HtmlDecode(titleNode?.InnerText.Trim()) ?? "",
                    Url = url,
                    ImageUrl = imageNode?.GetAttributeValue("src", "") ?? "",
                    Content = WebUtility.HtmlDecode(content)
                };

                newsList.Add(newsItem);
            }
        }
        return newsList;
    }
    private async Task<string> GetNewsContentAsync(string? contentUrl)
    {
        var newsBody = await _httpClient.GetStringAsync(contentUrl);

        var doc = new HtmlDocument();
        doc.LoadHtml(newsBody);

        var articleNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'article-body')]");
        if (articleNode is null)
            return "İçerik bulunamadı";

        var contentNodes = articleNode.ChildNodes.Where(n => n.Name == "p" || n.Name == "h2");
        if (contentNodes is null)
            return "Paragraf ya da başlık bulunamadı.";

        var content = string.Join("\n\n", contentNodes.Select(n => n.InnerText.Trim()));
        return content;
    }
}