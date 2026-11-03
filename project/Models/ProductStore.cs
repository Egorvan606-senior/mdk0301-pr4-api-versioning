namespace ApiVersioningLab.Ivanov.Models;

/// <summary>Учебные данные каталога для проверки контрактов v1 и v2.</summary>
public record Product(int Id, string Title, decimal Price, string Currency, int Stock, string Category);

public static class ProductStore
{
    public static IReadOnlyList<Product> Items { get; } = new[]
    {
        new Product(1, "Ноутбук Lenovo IdeaPad", 62990m, "RUB", 4, "Ноутбуки"),
        new Product(2, "Смартфон Samsung Galaxy A55", 32490m, "RUB", 12, "Смартфоны")
    };
}
