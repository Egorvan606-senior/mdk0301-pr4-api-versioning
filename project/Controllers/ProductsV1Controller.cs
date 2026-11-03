using Asp.Versioning;
using ApiVersioningLab.Ivanov.Models;
using Microsoft.AspNetCore.Mvc;
 
namespace ApiVersioningLab.Ivanov.Controllers;
 
/// <summary>Каталог товаров — версия API 1.0.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
public class ProductsV1Controller : ControllerBase
{
    /// <summary>Список товаров (базовый набор полей).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductV1Dto>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var result = ProductStore.Items.Select(p => new ProductV1Dto(p.Id, p.Title, p.Price));
        return Ok(result);
    }
 
    /// <summary>Товар по идентификатору.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductV1Dto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var p = ProductStore.Items.FirstOrDefault(x => x.Id == id);
        return p is null ? NotFound() : Ok(new ProductV1Dto(p.Id, p.Title, p.Price));
    }
}
