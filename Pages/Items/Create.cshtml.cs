using System.ComponentModel.DataAnnotations;
using Baraholka.Models;
using Baraholka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Baraholka.Pages.Items;

public class CreateModel : PageModel
{
    private readonly DataService _dataService;

    public CreateModel(DataService dataService)
    {
        _dataService = dataService;
    }

    [BindProperty]
    public Item Item { get; set; } = new();

    [BindProperty]
    public IFormFile? Image { get; set; }

    public List<SelectListItem> Categories { get; } = new()
    {
        new("Одежда", "одежда"),
        new("Электроника", "электроника"),
        new("Мебель", "мебель")
    };

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (Image != null && Image.Length > 2 * 1024 * 1024)
        {
            ModelState.AddModelError("Image", "Размер файла не должен превышать 2 МБ");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        _dataService.AddItem(Item, Image);
        TempData["Message"] = "Объявление успешно опубликовано!";
        return RedirectToPage("/Items/Details", new { id = Item.Id });
    }
}
