using PAW.Models.DTO;
namespace PAW.Web.Services;
public interface IRoleService : ICrudService<RoleDTO> {  }
public class RoleService(ApiClient client) : CrudService<RoleDTO>(client, "Role"), IRoleService
{
    
}
