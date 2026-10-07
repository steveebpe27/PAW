using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;
namespace PAW.Web.Controllers;
public class UserController(IUserService service, LookupService lookups, ILogger<UserController> logger)
    : CrudMvcController<UserDTO>(service, logger)
{
    protected override async Task PrepareAsync(UserDTO dto)
    {
        ViewData["RoleIdOptions"] = await lookups.GetAsync("Role");
    }
}
