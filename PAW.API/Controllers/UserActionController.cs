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
public class UserActionController(IUserActionRepository repository, ILogger<UserActionController> logger)
    : CrudApiController<UserAction, UserActionDTO>(repository, logger)
{
    protected override UserActionDTO Map(UserAction entity) => UserActionDTO.ConvertFrom(entity);
    protected override void Apply(UserActionDTO dto, UserAction entity)
    {
        dto.ApplyTo(entity);
    }
    protected override string Key(UserAction entity) => entity.CrudId.ToString(CultureInfo.InvariantCulture);
    protected override string Label(UserAction entity) => Convert.ToString(entity.Name, CultureInfo.InvariantCulture) ?? Key(entity);
    protected override async Task<Dictionary<string, string[]>> ValidateAsync(UserActionDTO dto, bool isNew)
    {
        var errors = new Dictionary<string, string[]>();
        if (dto.Id.HasValue && decimal.Truncate(dto.Id.Value) != dto.Id.Value)
            errors["Id"] = ["El SQL requiere un número entero."];
        return errors;
    }
}
