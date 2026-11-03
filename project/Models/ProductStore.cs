namespace ApiVersioningLab.Ivanov.Models;

/// <summary>Учебные данные каталога, восстановленные по результатам Swagger в отчёте.</summary>
public record Product(int Id, string Title, decimal Price, string Currency, int Stock, string Category);

public static class ProductStore
{
    public static IReadOnlyList<Product> Items { get; } = new[]
    {
        new Product(1, "Ноутбук Lenovo IdeaPad 5", 64990m, "RUB", 10, "Ноутбуки"),
        new Product(2, "Смартфон Samsung Galaxy A55", 32490m, "RUB", 0, "Смартфоны"),
        new Product(3, "Беспроводные наушники Sony WH-1000XM5", 29990m, "RUB", 5, "Аудио")
    };
}
