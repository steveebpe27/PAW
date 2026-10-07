using System.ComponentModel.DataAnnotations;
using PAW.Models;
namespace PAW.Models.DTO;
public class UserRoleDTO
{
    public int CrudId { get; set; }
    [Range(typeof(decimal), "0", "999999999999999999", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
    public decimal? Id { get; set; }
    [Range(typeof(decimal), "0", "999999999999999999", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
    public decimal? RoldId { get; set; }
    [Range(typeof(decimal), "0", "999999999999999999", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
    public decimal? UserId { get; set; }

    public static UserRoleDTO ConvertFrom(UserRole entity) => new()
    {
        CrudId = entity.CrudId,
        Id = entity.Id,
        RoldId = entity.RoldId,
        UserId = entity.UserId,
    };
    // The API assigns the route/identity key; posted data cannot replace it.
    public void ApplyTo(UserRole entity)
    {
        entity.Id = Id;
        entity.RoldId = RoldId;
        entity.UserId = UserId;
    }
}
