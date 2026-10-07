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
public class UserRoleController(IUserRoleRepository repository, ProductDbContext context, ILogger<UserRoleController> logger)
    : CrudApiController<UserRole, UserRoleDTO>(repository, logger)
{
    protected override UserRoleDTO Map(UserRole entity) => UserRoleDTO.ConvertFrom(entity);
    protected override void Apply(UserRoleDTO dto, UserRole entity)
    {
        dto.ApplyTo(entity);
    }
    protected override string Key(UserRole entity) => entity.CrudId.ToString(CultureInfo.InvariantCulture);
    protected override string Label(UserRole entity) => Convert.ToString(entity.CrudId, CultureInfo.InvariantCulture) ?? Key(entity);
    protected override async Task<Dictionary<string, string[]>> ValidateAsync(UserRoleDTO dto, bool isNew)
    {
        var errors = new Dictionary<string, string[]>();
        if (dto.RoldId is decimal roldid && (roldid < 1 || roldid > int.MaxValue || decimal.Truncate(roldid) != roldid ||
            !await context.Set<Role>().AnyAsync(x => x.RoleId == (int)roldid)))
            errors["RoldId"] = ["Seleccioná una referencia existente."];
        if (dto.UserId is decimal userid && (userid < 1 || userid > int.MaxValue || decimal.Truncate(userid) != userid ||
            !await context.Set<User>().AnyAsync(x => x.UserId == (int)userid)))
            errors["UserId"] = ["Seleccioná una referencia existente."];
        if (dto.Id.HasValue && decimal.Truncate(dto.Id.Value) != dto.Id.Value)
            errors["Id"] = ["El SQL requiere un número entero."];
        return errors;
    }
}
