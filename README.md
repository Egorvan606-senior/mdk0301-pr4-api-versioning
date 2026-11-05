# МДК 03.01 — ПР4: версионирование API каталога товаров

- [Отчёт DOCX](report/%D0%9F%D1%80%D0%B0%D0%BA%D1%82%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B0%D1%8F%20%D1%80%D0%B0%D0%B1%D0%BE%D1%82%D0%B0%204%20%D0%98%D0%B2%D0%B0%D0%BD%D0%BE%D0%B2%20%D0%95.%D0%9C..docx)
- [Отчёт PDF](report/%D0%9F%D1%80%D0%B0%D0%BA%D1%82%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B0%D1%8F%20%D1%80%D0%B0%D0%B1%D0%BE%D1%82%D0%B0%204%20%D0%98%D0%B2%D0%B0%D0%BD%D0%BE%D0%B2%20%D0%95.%D0%9C..pdf)
- [Архив проекта](archive/%D0%9F%D0%A04_%D0%92%D0%B5%D1%80%D1%81%D0%B8%D0%BE%D0%BD%D0%B8%D1%80%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5_API_%D0%BF%D1%80%D0%BE%D0%B5%D0%BA%D1%82_%D0%98%D0%B2%D0%B0%D0%BD%D0%BE%D0%B2.zip)
- [Исходный проект](project/)

Проверка проекта: сборка Visual Studio и .NET 10 без ошибок и предупреждений; 7/7 проверок HTTP API пройдено. Запуск и особенности проекта описаны в `project/README.md`. Все снимки в отчёте получены на устройстве автора; демонстрационные данные не являются производственными.

Скриншоты оригинальных запросов и ответов Swagger для v1 и v2, а также сборки в Visual Studio находятся в [screenshots](screenshots/). В проекте восстановлены три товара, показанные в отчёте.

## Быстрый запуск

1. На GitHub нажмите **Code → Download ZIP** и распакуйте репозиторий. Установите **.NET 10 SDK**. В Visual Studio откройте [`project/ApiVersioningLab.Ivanov.slnx`](project/ApiVersioningLab.Ivanov.slnx).
2. В PowerShell перейдите в папку `project` и выполните:

   ```powershell
   dotnet restore
   dotnet run --project ApiVersioningLab.Ivanov.csproj --urls http://localhost:5180
   ```

3. Откройте в браузере `http://localhost:5180/swagger`. Выберите документ **V1** или **V2**, раскройте `GET /api/v1/products` либо `GET /api/v2/products`, нажмите **Try it out → Execute**. Ниже появится JSON-ответ. Для остановки сервера нажмите **Ctrl+C** в PowerShell.

Прямые адреса: `http://localhost:5180/api/v1/products` и `http://localhost:5180/api/v2/products`. В `v1` каждый товар содержит `id`, `title`, `price`; в `v2` к ним добавлены `currency`, `inStock`, `category`, `apiVersionInfo`. Запрос `/api/v2/products/2` покажет товар № 2, а `/api/v2/products/999999` вернёт `404`.

## Где находится код

| Файл | Назначение |
| --- | --- |
| [`project/Program.cs`](project/Program.cs) | Настройка версионирования, контроллеров и Swagger; запуск веб-сервера. |
| [`project/Controllers/ProductsV1Controller.cs`](project/Controllers/ProductsV1Controller.cs) | Ответы API v1. |
| [`project/Controllers/ProductsV2Controller.cs`](project/Controllers/ProductsV2Controller.cs) | Ответы API v2. |
| [`project/Models/ProductStore.cs`](project/Models/ProductStore.cs) | Три демонстрационных товара. |
| [`project/Models/ProductDtos.cs`](project/Models/ProductDtos.cs) | Поля ответов v1 и v2. |
| [`project/ApiVersioningLab.Ivanov.http`](project/ApiVersioningLab.Ivanov.http) | Готовые HTTP-запросы для редактора с поддержкой `.http`. |
| [`project/verify_api.py`](project/verify_api.py) | Дополнительная автоматическая проверка HTTP API. |

Для автоматической проверки выполните `dotnet build` в папке `project`, затем `python verify_api.py` при установленном Python 3. Скрипт сам запускает сервер на свободном порту и завершает его; ожидаемый итог — `RESULT: 7/7 checks passed`. Данные каталога учебные; остатки на складе условные.
