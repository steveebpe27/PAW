using System.ComponentModel.DataAnnotations;
using PAW.Models;
namespace PAW.Models.DTO;
public class RoleDTO
{
    public int RoleId { get; set; }
    [StringLength(255)]
    public string? RoleName { get; set; }

    public static RoleDTO ConvertFrom(Role entity) => new()
    {
        RoleId = entity.RoleId,
        RoleName = entity.RoleName,
    };
    // The API assigns the route/identity key; posted data cannot replace it.
    public void ApplyTo(Role entity)
    {
        entity.RoleName = RoleName;
    }
}
