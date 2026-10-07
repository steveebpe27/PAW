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
public class CategoryController(ICategoryRepository repository, ProductDbContext context, ILogger<CategoryController> logger)
    : CrudApiController<Category, CategoryDTO>(repository, logger)
{
    protected override CategoryDTO Map(Category entity) => CategoryDTO.ConvertFrom(entity);
    protected override void Apply(CategoryDTO dto, Category entity)
    {
        dto.ApplyTo(entity);
    }
    protected override string Key(Category entity) => entity.CategoryId.ToString(CultureInfo.InvariantCulture);
    protected override string Label(Category entity) => Convert.ToString(entity.CategoryName, CultureInfo.InvariantCulture) ?? Key(entity);
    protected override Task<bool> HasReferencesAsync(Category entity) =>
        context.Products.AnyAsync(x => x.CategoryId == entity.CategoryId);
}
