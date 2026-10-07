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
public class SupplierController(ISupplierRepository repository, ProductDbContext context, ILogger<SupplierController> logger)
    : CrudApiController<Supplier, SupplierDTO>(repository, logger)
{
    protected override SupplierDTO Map(Supplier entity) => SupplierDTO.ConvertFrom(entity);
    protected override void Apply(SupplierDTO dto, Supplier entity)
    {
        dto.ApplyTo(entity);
    }
    protected override string Key(Supplier entity) => entity.SupplierId.ToString(CultureInfo.InvariantCulture);
    protected override string Label(Supplier entity) => Convert.ToString(entity.SupplierName, CultureInfo.InvariantCulture) ?? Key(entity);
    protected override Task<bool> HasReferencesAsync(Supplier entity) =>
        context.Products.AnyAsync(x => x.SupplierId == entity.SupplierId);
}
