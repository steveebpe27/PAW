namespace PAW.Models.DTO;
public class ProductDetailsDTO
{
    public ProductDTO Product { get; set; } = new();
    public InventoryDTO? Inventory { get; set; }
    public CategoryDTO? Category { get; set; }
    public SupplierDTO? Supplier { get; set; }
}
