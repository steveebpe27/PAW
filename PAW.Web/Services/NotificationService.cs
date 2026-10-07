using PAW.Models.DTO;
namespace PAW.Web.Services;
public interface INotificationService : ICrudService<NotificationDTO> {  }
public class NotificationService(ApiClient client) : CrudService<NotificationDTO>(client, "Notification"), INotificationService
{
    
}
