using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;
namespace PAW.Web.Controllers;
public class SupplierController(ISupplierService service, ILogger<SupplierController> logger)
    : CrudMvcController<SupplierDTO>(service, logger)
{
}
