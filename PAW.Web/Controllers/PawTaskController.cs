using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;
namespace PAW.Web.Controllers;
public class PawTaskController(IPawTaskService service, ILogger<PawTaskController> logger)
    : CrudMvcController<PawTaskDTO>(service, logger)
{
}
