using Asp.Versioning;
using ApiVersioningLab.Ivanov.Swagger;
 
var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllers();
 
// 1. Регистрация службы версионирования API
builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);          // версия по умолчанию
        options.AssumeDefaultVersionWhenUnspecified = true;         // если клиент не указал версию — v1
        options.ReportApiVersions = true;                           // заголовки api-supported-versions
        options.ApiVersionReader = new UrlSegmentApiVersionReader(); // версия читается из сегмента URL
    })
    .AddMvc()
    // 2. Интеграция с ApiExplorer: Swagger получает отдельную группу на каждую версию
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";            // имена групп: v1, v2
        options.SubstituteApiVersionInUrl = true;      // {version} подставляется в маршрут
    });
 
// 3. Генератор Swagger: документ для каждой версии формируется автоматически
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
 
var app = builder.Build();
 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Выпадающий список версий в Swagger UI: v1 и v2
        foreach (var description in app.DescribeApiVersions())
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                $"Каталог интернет-магазина API {description.GroupName.ToUpperInvariant()}");
        }
    });
}
 
app.UseAuthorization();
app.MapControllers();
 
app.Run();
