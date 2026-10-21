using PAW.Models;
using PAW.Models.DTO;

namespace PAW.MinimalAPI.Repositories;

public interface ITaskRepository
{
    Task<PageResult<PawTask>> GetPageAsync(int page, int pageSize, CancellationToken ct = default);
    Task<PawTask?> FindAsync(int id, CancellationToken ct = default);
    Task<PawTask> CreateAsync(PawTask task, CancellationToken ct = default);
    Task<PawTask?> UpdateAsync(int id, PawTask task, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
