using System.ComponentModel.DataAnnotations;
using PAW.Models;
namespace PAW.Models.DTO;
public class CategoryDTO
{
    public int CategoryId { get; set; }
    [StringLength(255)]
    public string? CategoryName { get; set; }
    [StringLength(1000)]
    public string? Description { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime? LastModified { get; set; }
    [StringLength(255)]
    public string? ModifiedBy { get; set; }

    public static CategoryDTO ConvertFrom(Category entity) => new()
    {
        CategoryId = entity.CategoryId,
        CategoryName = entity.CategoryName,
        Description = entity.Description,
        LastModified = entity.LastModified,
        ModifiedBy = entity.ModifiedBy,
    };
    // The API assigns the route/identity key; posted data cannot replace it.
    public void ApplyTo(Category entity)
    {
        entity.CategoryName = CategoryName;
        entity.Description = Description;
        entity.LastModified = LastModified;
        entity.ModifiedBy = ModifiedBy;
    }
}
