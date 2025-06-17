using ElasticNews.Application.Services;
using ElasticNews.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElasticNews.WebUI.Pages;

public class IndexModel(INewsService newsService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Query { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    public SearchViewModel ViewModel { get; set; } = new();
    public async Task OnGetAsync()
    {
        const int pageSize = 20;
        var (results, totalCount) = await newsService.SearchAsync(Query, PageNumber, pageSize);

        ViewModel = new SearchViewModel
        {
            Query = Query,
            Page = PageNumber,
            PageSize = pageSize,
            Results = results,
            TotalResults = totalCount
        };
    }
}
