using Microsoft.AspNetCore.Mvc;
using PAW.MinimalAPI.Contracts;
using PAW.MinimalAPI.Services;
using PAW.Models.DTO;

namespace PAW.MinimalAPI.Endpoints;

public static class TaskEndpoints
{
    public static IEndpointRouteBuilder MapTaskEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var tasks = endpoints.MapGroup("/tasks").WithTags("Tasks");

        tasks.MapGet("/", async ([FromServices] ITaskService service, CancellationToken ct, int page = 1, int pageSize = 25) =>
            Results.Ok(await service.GetPageAsync(page, pageSize, ct)))
            .WithName("GetTasks").Produces<PageResult<PawTaskDTO>>();

        tasks.MapGet("/{id:int:min(1)}", async (int id, [FromServices] ITaskService service, CancellationToken ct) =>
        {
            var task = await service.FindAsync(id, ct);
            return task is null ? Results.NotFound() : Results.Ok(task);
        }).WithName("GetTaskById").Produces<PawTaskDTO>().Produces(404);

        tasks.MapPost("/", async ([FromBody] TaskRequest request, [FromServices] ITaskService service, CancellationToken ct) =>
        {
            var errors = request.Validate();
            if (errors.Count > 0) return Results.ValidationProblem(errors);
            var task = await service.CreateAsync(request, ct);
            return Results.Created($"/tasks/{task.Id}", task);
        }).WithName("CreateTask").Produces<PawTaskDTO>(201).ProducesValidationProblem();

        tasks.MapPut("/{id:int:min(1)}", async (int id, [FromBody] TaskRequest request, [FromServices] ITaskService service, CancellationToken ct) =>
        {
            var errors = request.Validate();
            if (errors.Count > 0) return Results.ValidationProblem(errors);
            var task = await service.UpdateAsync(id, request, ct);
            return task is null ? Results.NotFound() : Results.NoContent();
        }).WithName("UpdateTask").Produces(204).Produces(404).ProducesValidationProblem();

        tasks.MapDelete("/{id:int:min(1)}", async (int id, [FromServices] ITaskService service, CancellationToken ct) =>
            await service.DeleteAsync(id, ct) ? Results.NoContent() : Results.NotFound())
            .WithName("DeleteTask").Produces(204).Produces(404).ProducesProblem(409);

        return endpoints;
    }
}
