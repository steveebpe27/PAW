using PAW.Models.DTO;
namespace PAW.Web.Services;
public interface IProductService : ICrudService<ProductDTO> { Task<ProductDetailsDTO?> DetailsAsync(string id); }
public class ProductService(ApiClient client) : CrudService<ProductDTO>(client, "Product"), IProductService
{
    public Task<ProductDetailsDTO?> DetailsAsync(string id) => Client.GetAsync<ProductDetailsDTO>($"Product/{Uri.EscapeDataString(id)}/details");
}
