using Baraholka.Models;
using Baraholka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Baraholka.Pages;

public class IndexModel : PageModel
{
    private readonly DataService _dataService;

    public IndexModel(DataService dataService)
    {
        _dataService = dataService;
    }

    public List<Item> Items { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Category { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    public void OnGet()
    {
        Items = _dataService.GetItems(Category, Search, activeOnly: true);
    }
}
