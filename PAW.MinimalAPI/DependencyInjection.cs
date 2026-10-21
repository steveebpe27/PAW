using PAW.MinimalAPI.Data;
using PAW.MinimalAPI.Repositories;
using PAW.MinimalAPI.Services;

namespace PAW.MinimalAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddTaskData(this IServiceCollection services)
    {
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<ITaskRepository, SqlTaskRepository>();
        services.AddScoped<ITaskService, TaskService>();
        return services;
    }
}
