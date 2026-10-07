using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;
namespace PAW.Web.Controllers;
public class InventoryController(IInventoryService service, LookupService lookups, ILogger<InventoryController> logger)
    : CrudMvcController<InventoryDTO>(service, logger)
{
    protected override async Task PrepareAsync(InventoryDTO dto)
    {
        ViewData["ProductIdOptions"] = await lookups.GetAsync("Product");
    }
}
