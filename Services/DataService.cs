using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Baraholka.Models;

namespace Baraholka.Services;

public class DataService
{
    private readonly string _itemsPath;
    private readonly string _ordersPath;
    private readonly string _uploadsPath;
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
    };

    public DataService(IWebHostEnvironment env)
    {
        var dataDir = Path.Combine(env.ContentRootPath, "Data");
        Directory.CreateDirectory(dataDir);

        _itemsPath = Path.Combine(dataDir, "items.json");
        _ordersPath = Path.Combine(dataDir, "orders.json");

        var webRoot = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
        _uploadsPath = Path.Combine(webRoot, "images", "uploads");
        Directory.CreateDirectory(_uploadsPath);

        Initialize();
    }

    private void Initialize()
    {
        if (!File.Exists(_itemsPath) || new FileInfo(_itemsPath).Length == 0)
        {
            var seed = new List<Item>
            {
                new()
                {
                    Id = 1,
                    Title = "Зимняя теплая куртка",
                    Description = "Теплая непродуваемая зимняя куртка, размер L. Идеальное состояние.",
                    Price = 6500m,
                    Category = "одежда",
                    ImageUrl = "/images/items/jacket.svg",
                    SellerName = "Алексей",
                    SellerPhone = "+7 (916) 123-45-67",
                    CreatedAt = DateTime.Now.AddDays(-2),
                    IsActive = true,
                    IsBooked = false
                },
                new()
                {
                    Id = 2,
                    Title = "Ноутбук для учебы и работы",
                    Description = "Надежный ноутбук, 16 ГБ RAM, 512 ГБ SSD. Батарея держит отлично.",
                    Price = 45000m,
                    Category = "электроника",
                    ImageUrl = "/images/items/laptop.svg",
                    SellerName = "Дмитрий",
                    SellerPhone = "+7 (903) 345-67-89",
                    CreatedAt = DateTime.Now.AddDays(-1),
                    IsActive = true,
                    IsBooked = false
                },
                new()
                {
                    Id = 3,
                    Title = "Офисное кресло",
                    Description = "Удобное рабочее кресло с регулировкой высоты и наклона.",
                    Price = 12000m,
                    Category = "мебель",
                    ImageUrl = "/images/items/chair.svg",
                    SellerName = "Сергей",
                    SellerPhone = "+7 (985) 678-90-12",
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    IsBooked = false
                }
            };
            SaveItems(seed);
        }

        if (!File.Exists(_ordersPath))
        {
            SaveOrders(new List<Order>());
        }
    }

    public List<Item> GetItems(string? category = null, string? search = null, bool activeOnly = true)
    {
        var items = GetAllItems();

        if (activeOnly)
        {
            items = items.Where(i => i.IsActive).ToList();
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            items = items.Where(i => string.Equals(i.Category, category.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            items = items.Where(i => i.Title.Contains(search.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
        }

        return items.OrderByDescending(i => i.CreatedAt).ToList();
    }

    public List<Item> GetAllItems()
    {
        if (!File.Exists(_itemsPath)) return new List<Item>();
        var json = File.ReadAllText(_itemsPath);
        return JsonSerializer.Deserialize<List<Item>>(json, _options) ?? new List<Item>();
    }

    public Item? GetItem(int id)
    {
        return GetAllItems().FirstOrDefault(i => i.Id == id);
    }

    public void AddItem(Item item, IFormFile? image)
    {
        var items = GetAllItems();
        item.Id = items.Count > 0 ? items.Max(i => i.Id) + 1 : 1;
        item.CreatedAt = DateTime.Now;
        item.IsActive = true;
        item.IsBooked = false;

        if (image != null && image.Length > 0)
        {
            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(_uploadsPath, fileName);
            using var stream = new FileStream(fullPath, FileMode.Create);
            image.CopyTo(stream);
            item.ImageUrl = $"/images/uploads/{fileName}";
        }
        else
        {
            item.ImageUrl = "/images/placeholder.svg";
        }

        items.Add(item);
        SaveItems(items);
    }

    public void TogglePublish(int id)
    {
        var items = GetAllItems();
        var item = items.FirstOrDefault(i => i.Id == id);
        if (item != null)
        {
            item.IsActive = !item.IsActive;
            SaveItems(items);
        }
    }

    public void DeleteItem(int id)
    {
        var items = GetAllItems();
        var item = items.FirstOrDefault(i => i.Id == id);
        if (item != null)
        {
            items.Remove(item);
            SaveItems(items);
        }
    }

    public void CreateOrder(int itemId, string name, string phone, string? comment)
    {
        var items = GetAllItems();
        var item = items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
        {
            item.IsBooked = true;
            SaveItems(items);

            var orders = GetAllOrders();
            var order = new Order
            {
                Id = orders.Count > 0 ? orders.Max(o => o.Id) + 1 : 1,
                ItemId = itemId,
                CustomerName = name,
                CustomerPhone = phone,
                Comment = comment,
                CreatedAt = DateTime.Now
            };
            orders.Add(order);
            SaveOrders(orders);
        }
    }

    public List<Order> GetAllOrders()
    {
        if (!File.Exists(_ordersPath)) return new List<Order>();
        var json = File.ReadAllText(_ordersPath);
        return JsonSerializer.Deserialize<List<Order>>(json, _options) ?? new List<Order>();
    }

    private void SaveItems(List<Item> items)
    {
        var json = JsonSerializer.Serialize(items, _options);
        File.WriteAllText(_itemsPath, json);
    }

    private void SaveOrders(List<Order> orders)
    {
        var json = JsonSerializer.Serialize(orders, _options);
        File.WriteAllText(_ordersPath, json);
    }
}
