namespace ApiVersioningLab.Ivanov.Models;
 
/// <summary>Контракт товара версии 1 — базовый набор полей.</summary>
public record ProductV1Dto(int Id, string Title, decimal Price);
 
/// <summary>
/// Контракт товара версии 2. Поля v1 сохранены без изменений (имя, тип, смысл),
/// новые поля только добавлены — аддитивное изменение.
/// </summary>
public record ProductV2Dto(
    int Id,
    string Title,
    decimal Price,
    string Currency,
    bool InStock,
    string Category,
    string ApiVersionInfo);
