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
public class ComponentController(IComponentRepository repository, ILogger<ComponentController> logger)
    : CrudApiController<Component, ComponentDTO>(repository, logger)
{
    protected override ComponentDTO Map(Component entity) => ComponentDTO.ConvertFrom(entity);
    protected override void Apply(ComponentDTO dto, Component entity)
    {
        dto.ApplyTo(entity);
    }
    protected override string Key(Component entity) => entity.Id.ToString(CultureInfo.InvariantCulture);
    protected override string Label(Component entity) => Convert.ToString(entity.Name, CultureInfo.InvariantCulture) ?? Key(entity);
}
