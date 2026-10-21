using PAW.MinimalAPI.Contracts;
using PAW.Models.DTO;

namespace PAW.MinimalAPI.Services;

public interface ITaskService
{
    Task<PageResult<PawTaskDTO>> GetPageAsync(int page, int pageSize, CancellationToken ct = default);
    Task<PawTaskDTO?> FindAsync(int id, CancellationToken ct = default);
    Task<PawTaskDTO> CreateAsync(TaskRequest request, CancellationToken ct = default);
    Task<PawTaskDTO?> UpdateAsync(int id, TaskRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
