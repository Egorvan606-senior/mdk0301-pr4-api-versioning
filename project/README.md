# ПР4 — версионирование API

Исходники восстановлены из листингов отчёта ПР4. В исходном отчёте отсутствовал листинг `Models/ProductStore.cs`, поэтому учебный набор данных добавлен отдельно. Данные демонстрационные.

```powershell
dotnet restore
dotnet run --urls http://localhost:5180
```

Проверка: `GET /api/v1/products`, `GET /api/v2/products`, `GET /api/v1/products/1`, `GET /api/v2/products/1`. Документация: `/swagger`.
