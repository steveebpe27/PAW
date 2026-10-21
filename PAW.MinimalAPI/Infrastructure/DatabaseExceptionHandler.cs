using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace PAW.MinimalAPI.Infrastructure;

public sealed class DatabaseExceptionHandler(ILogger<DatabaseExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        if (exception is not SqlException sql) return false;
        logger.LogError(sql, "No se pudo completar una operación de Tasks en SQL Server.");
        var conflict = sql.Number is 547 or 2601 or 2627;
        var problem = new ProblemDetails
        {
            Status = conflict ? StatusCodes.Status409Conflict : StatusCodes.Status503ServiceUnavailable,
            Title = conflict ? "La operación afecta registros asociados o viola una restricción. Revisá las referencias."
                : "No se pudo acceder a Tasks. Revisá SQL Server y la conexión de PAW.MinimalAPI."
        };
        context.Response.StatusCode = problem.Status.Value;
        await Results.Problem(problem).ExecuteAsync(context);
        return true;
    }
}
