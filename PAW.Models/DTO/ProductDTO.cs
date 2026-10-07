using System.ComponentModel.DataAnnotations;
using PAW.Models;
namespace PAW.Models.DTO;
public class ProductDTO
{
    public int ProductId { get; set; }
    [StringLength(255)]
    public string? Name { get; set; }
    public int? InventoryId { get; set; }
    public int? SupplierId { get; set; }
    [StringLength(1000)]
    public string? Description { get; set; }
    [Range(typeof(decimal), "0", "5", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
    public decimal? Rating { get; set; }
    public int? CategoryId { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime? LastModified { get; set; }
    [StringLength(255)]
    public string? ModifiedBy { get; set; }
    [StringLength(100)]
    public string? CreatedBy { get; set; }
    // Compatibility display fields: the SQL schema has no Comments/CreatedDate columns.
    public string Comments => string.Empty;
    public DateTime? CreatedDate => null;
    public DateTime? ModifiedDate => LastModified;

    public static ProductDTO ConvertFrom(Product entity) => new()
    {
        ProductId = entity.ProductId,
        Name = entity.ProductName,
        InventoryId = entity.InventoryId,
        SupplierId = entity.SupplierId,
        Description = entity.Description,
        Rating = entity.Rating,
        CategoryId = entity.CategoryId,
        LastModified = entity.LastModified,
        ModifiedBy = entity.ModifiedBy,
        CreatedBy = entity.CreatedBy,
    };
    // The API assigns the route/identity key; posted data cannot replace it.
    public void ApplyTo(Product entity)
    {
        entity.ProductName = Name;
        entity.InventoryId = InventoryId;
        entity.SupplierId = SupplierId;
        entity.Description = Description;
        entity.Rating = Rating;
        entity.CategoryId = CategoryId;
        entity.LastModified = LastModified;
        entity.ModifiedBy = ModifiedBy;
        entity.CreatedBy = CreatedBy;
    }
}
