using Asp.Versioning;
using ApiVersioningLab.Ivanov.Models;
using Microsoft.AspNetCore.Mvc;
 
namespace ApiVersioningLab.Ivanov.Controllers;
 
/// <summary>Каталог товаров — версия API 2.0 (расширяет v1 аддитивными полями).</summary>
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsV2Controller : ControllerBase
{
    private const string VersionInfo =
        "v2.0: добавлены поля currency, inStock, category; поля v1 не изменены";
 
    /// <summary>Список товаров (поля v1 + новые поля).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductV2Dto>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var result = ProductStore.Items.Select(Map);
        return Ok(result);
    }
 
    /// <summary>Товар по идентификатору.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductV2Dto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var p = ProductStore.Items.FirstOrDefault(x => x.Id == id);
        return p is null ? NotFound() : Ok(Map(p));
    }
 
    private static ProductV2Dto Map(Product p) =>
        new(p.Id, p.Title, p.Price, p.Currency, p.Stock > 0, p.Category, VersionInfo);
}
