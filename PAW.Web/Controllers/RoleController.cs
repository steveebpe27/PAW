using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;
namespace PAW.Web.Controllers;
public class RoleController(IRoleService service, ILogger<RoleController> logger)
    : CrudMvcController<RoleDTO>(service, logger)
{
}
