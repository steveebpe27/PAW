using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAW.DataAccess.MSSQL;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;
namespace PAW.API.Controllers;
[ApiController]
[Route("[controller]")]
public class RoleController(IRoleRepository repository, ProductDbContext context, ILogger<RoleController> logger)
    : CrudApiController<Role, RoleDTO>(repository, logger)
{
    protected override RoleDTO Map(Role entity) => RoleDTO.ConvertFrom(entity);
    protected override void Apply(RoleDTO dto, Role entity)
    {
        dto.ApplyTo(entity);
    }
    protected override string Key(Role entity) => entity.RoleId.ToString(CultureInfo.InvariantCulture);
    protected override string Label(Role entity) => Convert.ToString(entity.RoleName, CultureInfo.InvariantCulture) ?? Key(entity);
    protected override async Task<bool> HasReferencesAsync(Role entity) =>
        await context.Users.AnyAsync(x => x.RoleId == entity.RoleId) ||
        await context.UserRoles.AnyAsync(x => x.RoldId == entity.RoleId);
}
