using System.ComponentModel.DataAnnotations;
using PAW.Models;
namespace PAW.Models.DTO;
public class InventoryDTO
{
    public int InventoryId { get; set; }
    [Range(typeof(decimal), "0", "99999999.99", ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
    public decimal? UnitPrice { get; set; }
    [Range(0, int.MaxValue)]
    public int? UnitsInStock { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime? LastUpdated { get; set; }
    public int? ProductId { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime? DateAdded { get; set; }
    [StringLength(255)]
    public string? ModifiedBy { get; set; }

    public static InventoryDTO ConvertFrom(Inventory entity) => new()
    {
        InventoryId = entity.InventoryId,
        UnitPrice = entity.UnitPrice,
        UnitsInStock = entity.UnitsInStock,
        LastUpdated = entity.LastUpdated,
        ProductId = entity.ProductId,
        DateAdded = entity.DateAdded,
        ModifiedBy = entity.ModifiedBy,
    };
    // The API assigns the route/identity key; posted data cannot replace it.
    public void ApplyTo(Inventory entity)
    {
        entity.UnitPrice = UnitPrice;
        entity.UnitsInStock = UnitsInStock;
        entity.LastUpdated = LastUpdated;
        entity.ProductId = ProductId;
        entity.DateAdded = DateAdded;
        entity.ModifiedBy = ModifiedBy;
    }
}
