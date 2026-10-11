using System.ComponentModel.DataAnnotations;

namespace Baraholka.Models;

public class Item
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [StringLength(100, ErrorMessage = "Максимальная длина — 100 символов")]
    [Display(Name = "Название")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [StringLength(2000, ErrorMessage = "Максимальная длина — 2000 символов")]
    [Display(Name = "Описание")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [Range(0.01, 100000000, ErrorMessage = "Цена должна быть больше 0")]
    [Display(Name = "Цена")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [Display(Name = "Категория")]
    public string Category { get; set; } = "одежда";

    [Display(Name = "Изображение")]
    public string ImageUrl { get; set; } = "/images/placeholder.svg";

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [StringLength(100, ErrorMessage = "Максимальная длина — 100 символов")]
    [Display(Name = "Имя продавца")]
    public string SellerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [StringLength(20, ErrorMessage = "Максимальная длина — 20 символов")]
    [Display(Name = "Телефон продавца")]
    public string SellerPhone { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public bool IsActive { get; set; } = true;

    public bool IsBooked { get; set; } = false;
}
