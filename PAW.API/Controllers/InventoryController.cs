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
public class InventoryController(IInventoryRepository repository, ProductDbContext context, ILogger<InventoryController> logger)
    : CrudApiController<Inventory, InventoryDTO>(repository, logger)
{
    protected override InventoryDTO Map(Inventory entity) => InventoryDTO.ConvertFrom(entity);
    protected override void Apply(InventoryDTO dto, Inventory entity)
    {
        dto.ApplyTo(entity);
    }
    protected override string Key(Inventory entity) => entity.InventoryId.ToString(CultureInfo.InvariantCulture);
    protected override string Label(Inventory entity) => Convert.ToString(entity.InventoryId, CultureInfo.InvariantCulture) ?? Key(entity);
    protected override Task<bool> HasReferencesAsync(Inventory entity) =>
        context.Products.AnyAsync(x => x.InventoryId == entity.InventoryId);
    protected override async Task<Dictionary<string, string[]>> ValidateAsync(InventoryDTO dto, bool isNew)
    {
        var errors = new Dictionary<string, string[]>();
        if (dto.ProductId.HasValue && !await context.Set<Product>().AnyAsync(x => x.ProductId == dto.ProductId.Value))
            errors["ProductId"] = ["Seleccioná una referencia existente."];
        return errors;
    }
}
