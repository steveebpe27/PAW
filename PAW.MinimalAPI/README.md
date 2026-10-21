# PAW.MinimalAPI — Tasks

ASP.NET Core Minimal API (.NET 10). Endpoints con MapGroup/MapGet/MapPost/MapPut/MapDelete; no hay controllers en este proyecto.

Ruta de dependencias: endpoints → ITaskService/TaskService → ITaskRepository/SqlTaskRepository → ISqlConnectionFactory/SqlConnectionFactory → dbo.Tasks en ProductDB.

No utiliza DbContext, EF Core ni PAW.DataAccess. Solo reutiliza PawTask, PawTaskDTO y PageResult del proyecto de modelos. SQL parametrizado con Microsoft.Data.SqlClient. Los datos persisten en tu SQL Server.

## Ejecutar

1. SDK .NET 10 y, para abrir este proyecto en el IDE, Visual Studio compatible con .NET 10 (el tutorial usa Visual Studio 2026).
2. Revisar ConnectionStrings:ProductDB en appsettings.json. Se usa el esquema existente de dbo.Tasks; esta tarea no requiere recrear tablas.
3. Ejecutar `dotnet run --project PAW.MinimalAPI --launch-profile https` desde la solución.
4. API: https://localhost:7210/tasks. OpenAPI en Development: https://localhost:7210/openapi/v1.json.
5. PAW.Web configura MinimalApiBaseUrl con ese origen. El menú Tasks abre /Tasks/Index y todos sus métodos usan esta API.

El archivo PAW.MinimalAPI.http contiene solicitudes de prueba. Reemplazá @id por el registro que creaste antes de editar/eliminar; no uses un Id de datos reales ajenos a la prueba. PUT reemplaza todos los campos editables. La creación y última modificación se asignan en UTC por SQL Server; CreatedAt se conserva al editar.

GET /tasks devuelve una página con items y totalCount. pageSize se limita a 25. GET /tasks/{id}, POST /tasks, PUT /tasks/{id} y DELETE /tasks/{id} completan el CRUD.

DI/IoC no es un motor de datos: permite sustituir el repositorio por otra implementación. En ejecución normal se registra SqlTaskRepository, no un repositorio temporal de memoria.
