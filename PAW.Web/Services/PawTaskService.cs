using PAW.Models.DTO;
namespace PAW.Web.Services;
public interface IPawTaskService : ICrudService<PawTaskDTO> {  }
public class PawTaskService(ApiClient client) : CrudService<PawTaskDTO>(client, "PawTask"), IPawTaskService
{
    
}
