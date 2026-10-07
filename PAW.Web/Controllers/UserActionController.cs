using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;
namespace PAW.Web.Controllers;
public class UserActionController(IUserActionService service, ILogger<UserActionController> logger)
    : CrudMvcController<UserActionDTO>(service, logger)
{
}
