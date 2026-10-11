using System.ComponentModel.DataAnnotations;
using Baraholka.Models;
using Baraholka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Baraholka.Pages.Items;

public class DetailsModel : PageModel
{
    private readonly DataService _dataService;

    public DetailsModel(DataService dataService)
    {
        _dataService = dataService;
    }

    public Item Item { get; set; } = null!;

    [BindProperty]
    public OrderInput Order { get; set; } = new();

    public class OrderInput
    {
        [Required(ErrorMessage = "Введите ваше имя")]
        [StringLength(100, ErrorMessage = "Максимальная длина — 100 символов")]
        [Display(Name = "Ваше имя")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите номер телефона")]
        [StringLength(20, ErrorMessage = "Максимальная длина — 20 символов")]
        [Display(Name = "Номер телефона")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Максимальная длина — 500 символов")]
        [Display(Name = "Комментарий")]
        public string? Comment { get; set; }
    }

    public IActionResult OnGet(int id)
    {
        var item = _dataService.GetItem(id);
        if (item == null) return NotFound();

        Item = item;
        return Page();
    }

    public IActionResult OnPostOrder(int id)
    {
        var item = _dataService.GetItem(id);
        if (item == null) return NotFound();

        Item = item;

        if (!ModelState.IsValid)
        {
            ViewData["ShowModal"] = true;
            return Page();
        }

        _dataService.CreateOrder(id, Order.Name, Order.Phone, Order.Comment);
        TempData["Message"] = "Заказ успешно оформлен! Товар забронирован.";
        return RedirectToPage("/Items/Details", new { id });
    }

    public IActionResult OnPostToggle(int id)
    {
        _dataService.TogglePublish(id);
        TempData["Message"] = "Статус публикации изменен.";
        return RedirectToPage("/Items/Details", new { id });
    }

    public IActionResult OnPostDelete(int id)
    {
        _dataService.DeleteItem(id);
        TempData["Message"] = "Объявление удалено.";
        return RedirectToPage("/Index");
    }
}
