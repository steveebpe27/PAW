using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAW.DataAccess.MSSQL;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;
namespace PAW.API.Controllers;
[ApiController]
[Route("[controller]")]
public class ProductController(IProductRepository repository, ProductDbContext context, ILogger<ProductController> logger)
    : CrudApiController<Product, ProductDTO>(repository, logger)
{
    protected override ProductDTO Map(Product entity) => ProductDTO.ConvertFrom(entity);
    protected override void Apply(ProductDTO dto, Product entity)
    {
        dto.ApplyTo(entity);
    }
    protected override string Key(Product entity) => entity.ProductId.ToString(CultureInfo.InvariantCulture);
    protected override string Label(Product entity) => Convert.ToString(entity.ProductName, CultureInfo.InvariantCulture) ?? Key(entity);
    protected override Task<bool> HasReferencesAsync(Product entity) =>
        context.Inventories.AnyAsync(x => x.ProductId == entity.ProductId);
    protected override async Task<Dictionary<string, string[]>> ValidateAsync(ProductDTO dto, bool isNew)
    {
        var errors = new Dictionary<string, string[]>();
        if (dto.CategoryId.HasValue && !await context.Set<Category>().AnyAsync(x => x.CategoryId == dto.CategoryId.Value))
            errors["CategoryId"] = ["Seleccioná una referencia existente."];
        if (dto.InventoryId.HasValue && !await context.Set<Inventory>().AnyAsync(x => x.InventoryId == dto.InventoryId.Value))
            errors["InventoryId"] = ["Seleccioná una referencia existente."];
        if (dto.SupplierId.HasValue && !await context.Set<Supplier>().AnyAsync(x => x.SupplierId == dto.SupplierId.Value))
            errors["SupplierId"] = ["Seleccioná una referencia existente."];
        return errors;
    }
    [HttpGet("{id}/details")]
    public async Task<ActionResult<ProductDetailsDTO>> Details(string id)
    {
        if (!int.TryParse(id, out var key)) return NotFound();
        var product = await context.Products.AsNoTracking().Include(x => x.Inventory)
            .Include(x => x.Category).Include(x => x.Supplier).SingleOrDefaultAsync(x => x.ProductId == key);
        if (product is null) return NotFound();
        return new ProductDetailsDTO { Product = ProductDTO.ConvertFrom(product),
            Inventory = product.Inventory is null ? null : InventoryDTO.ConvertFrom(product.Inventory),
            Category = product.Category is null ? null : CategoryDTO.ConvertFrom(product.Category),
            Supplier = product.Supplier is null ? null : SupplierDTO.ConvertFrom(product.Supplier) };
    }
}
