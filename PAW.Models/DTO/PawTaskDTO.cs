using System.ComponentModel.DataAnnotations;
using PAW.Models;
namespace PAW.Models.DTO;
public class PawTaskDTO
{
    public int Id { get; set; }
    [StringLength(255)]
    public string? Name { get; set; }
    [StringLength(1000)]
    public string? Description { get; set; }
    [StringLength(50)]
    public string? Status { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime? DueDate { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime? CreatedAt { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime? LastModified { get; set; }
    [StringLength(255)]
    public string? ModifiedBy { get; set; }

    public static PawTaskDTO ConvertFrom(PawTask entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        Status = entity.Status,
        DueDate = entity.DueDate,
        CreatedAt = entity.CreatedAt,
        LastModified = entity.LastModified,
        ModifiedBy = entity.ModifiedBy,
    };
    // The API assigns the route/identity key; posted data cannot replace it.
    public void ApplyTo(PawTask entity)
    {
        entity.Name = Name;
        entity.Description = Description;
        entity.Status = Status;
        entity.DueDate = DueDate;
        entity.CreatedAt = CreatedAt;
        entity.LastModified = LastModified;
        entity.ModifiedBy = ModifiedBy;
    }
}
