using PAW.MinimalAPI;
using PAW.MinimalAPI.Endpoints;
using PAW.MinimalAPI.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTaskData();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<DatabaseExceptionHandler>();

var app = builder.Build();
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    StatusCodeSelector = exception => exception is BadHttpRequestException badRequest
        ? badRequest.StatusCode : StatusCodes.Status500InternalServerError
});
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.MapGet("/", () => Results.Ok(new
{
    application = "PAW.MinimalAPI", entity = "Tasks", list = "/tasks", openApi = "/openapi/v1.json"
}));
app.MapTaskEndpoints();
app.Run();
