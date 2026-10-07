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
public class NotificationController(INotificationRepository repository, ProductDbContext context, ILogger<NotificationController> logger)
    : CrudApiController<Notification, NotificationDTO>(repository, logger)
{
    protected override NotificationDTO Map(Notification entity) => NotificationDTO.ConvertFrom(entity);
    protected override void Apply(NotificationDTO dto, Notification entity)
    {
        dto.ApplyTo(entity);
    }
    protected override string Key(Notification entity) => entity.Id.ToString(CultureInfo.InvariantCulture);
    protected override string Label(Notification entity) => Convert.ToString(entity.Message, CultureInfo.InvariantCulture) ?? Key(entity);
    protected override async Task<Dictionary<string, string[]>> ValidateAsync(NotificationDTO dto, bool isNew)
    {
        var errors = new Dictionary<string, string[]>();
        if (!await context.Set<User>().AnyAsync(x => x.UserId == dto.UserId))
            errors["UserId"] = ["Seleccioná una referencia existente."];
        return errors;
    }
}
