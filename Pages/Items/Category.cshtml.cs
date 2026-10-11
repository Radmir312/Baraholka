using Baraholka.Models;
using Baraholka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Baraholka.Pages.Items;

public class CategoryModel : PageModel
{
    private readonly DataService _dataService;

    public CategoryModel(DataService dataService)
    {
        _dataService = dataService;
    }

    public string CategoryName { get; set; } = string.Empty;
    public List<Item> Items { get; set; } = new();

    public void OnGet(string categoryName)
    {
        CategoryName = categoryName;
        Items = _dataService.GetItems(category: categoryName, activeOnly: true);
    }
}
