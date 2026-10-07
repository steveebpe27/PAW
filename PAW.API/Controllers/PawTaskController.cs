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
public class PawTaskController(IPawTaskRepository repository, ILogger<PawTaskController> logger)
    : CrudApiController<PawTask, PawTaskDTO>(repository, logger)
{
    protected override PawTaskDTO Map(PawTask entity) => PawTaskDTO.ConvertFrom(entity);
    protected override void Apply(PawTaskDTO dto, PawTask entity)
    {
        dto.ApplyTo(entity);
    }
    protected override string Key(PawTask entity) => entity.Id.ToString(CultureInfo.InvariantCulture);
    protected override string Label(PawTask entity) => Convert.ToString(entity.Name, CultureInfo.InvariantCulture) ?? Key(entity);
}
