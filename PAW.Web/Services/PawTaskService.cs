using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IPawTaskService : ICrudService<PawTaskDTO> { }

public class PawTaskService(MinimalApiClient client)
    : CrudService<PawTaskDTO>(client, "tasks"), IPawTaskService
{
}
