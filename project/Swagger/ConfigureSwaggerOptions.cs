using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
 
namespace ApiVersioningLab.Ivanov.Swagger;
 
/// <summary>
/// Создаёт отдельный Swagger-документ для каждой обнаруженной версии API.
/// </summary>
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
 
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;
 
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = "Каталог интернет-магазина API",
                Version = description.ApiVersion.ToString(),
                Description = description.ApiVersion.MajorVersion == 1
                    ? "Версия 1: базовый набор полей товара (id, title, price)."
                    : "Версия 2: поля v1 + аддитивные поля (currency, inStock, category, apiVersionInfo)."
            });
        }
    }
}
