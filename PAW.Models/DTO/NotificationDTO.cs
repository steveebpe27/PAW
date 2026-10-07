using System.ComponentModel.DataAnnotations;
using PAW.Models;
namespace PAW.Models.DTO;
public class NotificationDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    [Required]
    [StringLength(255)]
    public string Message { get; set; } = string.Empty;
    public bool? IsRead { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime? CreatedAt { get; set; }

    public static NotificationDTO ConvertFrom(Notification entity) => new()
    {
        Id = entity.Id,
        UserId = entity.UserId,
        Message = entity.Message,
        IsRead = entity.IsRead,
        CreatedAt = entity.CreatedAt,
    };
    // The API assigns the route/identity key; posted data cannot replace it.
    public void ApplyTo(Notification entity)
    {
        entity.UserId = UserId;
        entity.Message = Message;
        entity.IsRead = IsRead;
        entity.CreatedAt = CreatedAt;
    }
}
