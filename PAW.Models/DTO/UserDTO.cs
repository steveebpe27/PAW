using System.ComponentModel.DataAnnotations;
using PAW.Models;
namespace PAW.Models.DTO;
public class UserDTO
{
    public int UserId { get; set; }
    [StringLength(255)]
    public string? Username { get; set; }
    [StringLength(255)]
    [EmailAddress]
    public string? Email { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime? CreatedAt { get; set; }
    public bool? IsActive { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime? LastModified { get; set; }
    [StringLength(255)]
    public string? ModifiedBy { get; set; }
    public int? RoleId { get; set; }
    [StringLength(50)]
    public string? LastModifiedBy { get; set; }
    [StringLength(128, MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    public static UserDTO ConvertFrom(User entity) => new()
    {
        UserId = entity.UserId,
        Username = entity.Username,
        Email = entity.Email,
        CreatedAt = entity.CreatedAt,
        IsActive = entity.IsActive,
        LastModified = entity.LastModified,
        ModifiedBy = entity.ModifiedBy,
        RoleId = entity.RoleId,
        LastModifiedBy = entity.LastModifiedBy,
    };
    // The API assigns the route/identity key; posted data cannot replace it.
    public void ApplyTo(User entity)
    {
        entity.Username = Username;
        entity.Email = Email;
        entity.CreatedAt = CreatedAt;
        entity.IsActive = IsActive;
        entity.LastModified = LastModified;
        entity.ModifiedBy = ModifiedBy;
        entity.RoleId = RoleId;
        entity.LastModifiedBy = LastModifiedBy;
    }
}
