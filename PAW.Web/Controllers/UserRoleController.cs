using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;
namespace PAW.Web.Controllers;
public class UserRoleController(IUserRoleService service, LookupService lookups, ILogger<UserRoleController> logger)
    : CrudMvcController<UserRoleDTO>(service, logger)
{
    protected override async Task PrepareAsync(UserRoleDTO dto)
    {
        ViewData["RoldIdOptions"] = await lookups.GetAsync("Role");
        ViewData["UserIdOptions"] = await lookups.GetAsync("User");
    }
}
