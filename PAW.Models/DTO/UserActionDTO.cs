using System.ComponentModel.DataAnnotations;
using PAW.Models;
namespace PAW.Models.DTO;
public class UserActionDTO
{
    public int CrudId { get; set; }
    [Range(typeof(decimal), "0", "999999999999999999", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
    public decimal? Id { get; set; }
    [StringLength(50)]
    public string? Name { get; set; }
    [StringLength(100)]
    public string? Description { get; set; }

    public static UserActionDTO ConvertFrom(UserAction entity) => new()
    {
        CrudId = entity.CrudId,
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
    };
    // The API assigns the route/identity key; posted data cannot replace it.
    public void ApplyTo(UserAction entity)
    {
        entity.Id = Id;
        entity.Name = Name;
        entity.Description = Description;
    }
}
