using PAW.Models.DTO;
namespace PAW.Web.Services;
public interface ISupplierService : ICrudService<SupplierDTO> {  }
public class SupplierService(ApiClient client) : CrudService<SupplierDTO>(client, "Supplier"), ISupplierService
{
    
}
