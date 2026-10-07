using PAW.Models.DTO;
namespace PAW.Web.Services;
public interface IUserService : ICrudService<UserDTO> {  }
public class UserService(ApiClient client) : CrudService<UserDTO>(client, "User"), IUserService
{
    
}
