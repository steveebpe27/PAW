using PAW.MinimalAPI.Contracts;
using PAW.MinimalAPI.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.MinimalAPI.Services;

public sealed class TaskService(ITaskRepository repository) : ITaskService
{
    public async Task<PageResult<PawTaskDTO>> GetPageAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var result = await repository.GetPageAsync(page, pageSize, ct);
        return new PageResult<PawTaskDTO> { Items = result.Items.Select(PawTaskDTO.ConvertFrom).ToList(),
            Page = result.Page, PageSize = result.PageSize, TotalCount = result.TotalCount };
    }
    public async Task<PawTaskDTO?> FindAsync(int id, CancellationToken ct = default)
    {
        var entity = await repository.FindAsync(id, ct);
        return entity is null ? null : PawTaskDTO.ConvertFrom(entity);
    }
    public async Task<PawTaskDTO> CreateAsync(TaskRequest request, CancellationToken ct = default) =>
        PawTaskDTO.ConvertFrom(await repository.CreateAsync(ToEntity(request), ct));
    public async Task<PawTaskDTO?> UpdateAsync(int id, TaskRequest request, CancellationToken ct = default)
    {
        var entity = await repository.UpdateAsync(id, ToEntity(request), ct);
        return entity is null ? null : PawTaskDTO.ConvertFrom(entity);
    }
    public Task<bool> DeleteAsync(int id, CancellationToken ct = default) => repository.DeleteAsync(id, ct);
    private static PawTask ToEntity(TaskRequest request) => new()
    {
        Name = request.Name.Trim(), Description = request.Description, Status = request.Status,
        DueDate = request.DueDate, ModifiedBy = request.ModifiedBy
    };
}
