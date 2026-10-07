using PAW.Models.DTO;
namespace PAW.Web.Services;
public interface ICategoryService : ICrudService<CategoryDTO> {  }
public class CategoryService(ApiClient client) : CrudService<CategoryDTO>(client, "Category"), ICategoryService
{
    
}
