using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Controllers;

public class TasksController(IPawTaskService service, ILogger<TasksController> logger)
    : CrudMvcController<PawTaskDTO>(service, logger)
{
}
