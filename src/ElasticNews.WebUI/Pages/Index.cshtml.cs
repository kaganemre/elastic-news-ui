using ElasticNews.Application.Services;
using ElasticNews.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElasticNews.WebUI.Pages;

public class IndexModel(INewsService newsService) : PageModel
{
    public IEnumerable<News> NewsList { get; set; } = default!;
    public async Task OnGetAsync(string search)
    {
        NewsList = await newsService.GetAllNewsAsync();

        if (!string.IsNullOrEmpty(search))
        {
            NewsList = await newsService.SearchByTitleAsync(search);
        }
    }
}
