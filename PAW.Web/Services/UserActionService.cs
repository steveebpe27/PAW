using PAW.Models.DTO;
namespace PAW.Web.Services;
public interface IUserActionService : ICrudService<UserActionDTO> {  }
public class UserActionService(ApiClient client) : CrudService<UserActionDTO>(client, "UserAction"), IUserActionService
{
    
}
