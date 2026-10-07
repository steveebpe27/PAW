using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;
namespace PAW.Web.Controllers;
public class ComponentController(IComponentService service, ILogger<ComponentController> logger)
    : CrudMvcController<ComponentDTO>(service, logger)
{
}
