using PAW.Models.DTO;
namespace PAW.Web.Services;
public interface IUserRoleService : ICrudService<UserRoleDTO> {  }
public class UserRoleService(ApiClient client) : CrudService<UserRoleDTO>(client, "UserRole"), IUserRoleService
{
    
}
