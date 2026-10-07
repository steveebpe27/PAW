using PAW.Models.DTO;
namespace PAW.Web.Services;
public interface IInventoryService : ICrudService<InventoryDTO> {  }
public class InventoryService(ApiClient client) : CrudService<InventoryDTO>(client, "Inventory"), IInventoryService
{
    
}
