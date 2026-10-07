using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;
namespace PAW.Web.Controllers;
public class ProductController(IProductService service, LookupService lookups, ILogger<ProductController> logger)
    : CrudMvcController<ProductDTO>(service, logger)
{
    protected override async Task PrepareAsync(ProductDTO dto)
    {
        ViewData["CategoryIdOptions"] = await lookups.GetAsync("Category");
        ViewData["InventoryIdOptions"] = await lookups.GetAsync("Inventory");
        ViewData["SupplierIdOptions"] = await lookups.GetAsync("Supplier");
    }
    [HttpGet]
    public override async Task<IActionResult> Details([FromRoute] string id)
    {
        try
        {
            var result = await service.DetailsAsync(id);
            return result is null ? NotFound() : PartialView("_Details", result);
        }
        catch (ApiException ex) { return StatusCode((int)ex.Status, ex.Message); }
    }
}
