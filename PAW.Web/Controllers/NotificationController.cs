using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Services;
namespace PAW.Web.Controllers;
public class NotificationController(INotificationService service, LookupService lookups, ILogger<NotificationController> logger)
    : CrudMvcController<NotificationDTO>(service, logger)
{
    protected override async Task PrepareAsync(NotificationDTO dto)
    {
        ViewData["UserIdOptions"] = await lookups.GetAsync("User");
    }
}
