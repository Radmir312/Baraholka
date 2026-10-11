using System.ComponentModel.DataAnnotations;

namespace Baraholka.Models;

public class Order
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [StringLength(100, ErrorMessage = "Максимальная длина — 100 символов")]
    [Display(Name = "Имя")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [StringLength(20, ErrorMessage = "Максимальная длина — 20 символов")]
    [Display(Name = "Телефон")]
    public string CustomerPhone { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Максимальная длина — 500 символов")]
    [Display(Name = "Комментарий")]
    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
