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
public class UserController(IUserRepository repository, ProductDbContext context, ILogger<UserController> logger)
    : CrudApiController<User, UserDTO>(repository, logger)
{
    protected override UserDTO Map(User entity) => UserDTO.ConvertFrom(entity);
    protected override void Apply(UserDTO dto, User entity)
    {
        dto.ApplyTo(entity);
        if (!string.IsNullOrEmpty(dto.NewPassword))
            entity.PasswordHash = new Microsoft.AspNetCore.Identity.PasswordHasher<User>().HashPassword(entity, dto.NewPassword);
    }
    protected override string Key(User entity) => entity.UserId.ToString(CultureInfo.InvariantCulture);
    protected override string Label(User entity) => Convert.ToString(entity.Username, CultureInfo.InvariantCulture) ?? Key(entity);
    protected override async Task<bool> HasReferencesAsync(User entity) =>
        await context.Notifications.AnyAsync(x => x.UserId == entity.UserId) ||
        await context.UserRoles.AnyAsync(x => x.UserId == entity.UserId);
    protected override async Task<Dictionary<string, string[]>> ValidateAsync(UserDTO dto, bool isNew)
    {
        var errors = new Dictionary<string, string[]>();
        if (dto.RoleId.HasValue && !await context.Set<Role>().AnyAsync(x => x.RoleId == dto.RoleId.Value))
            errors["RoleId"] = ["Seleccioná una referencia existente."];
        if (isNew && string.IsNullOrWhiteSpace(dto.NewPassword))
            errors["NewPassword"] = ["Ingresá una contraseña de al menos 8 caracteres."];
        return errors;
    }
}
