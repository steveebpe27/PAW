using PAW.Models.DTO;
namespace PAW.Web.Services;
public interface IComponentService : ICrudService<ComponentDTO> {  }
public class ComponentService(ApiClient client) : CrudService<ComponentDTO>(client, "Component"), IComponentService
{
    
}
