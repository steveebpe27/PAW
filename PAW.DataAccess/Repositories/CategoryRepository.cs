using PAW.DataAccess.MSSQL;
using PAW.Models;
namespace PAW.DataAccess.Repositories;
public interface ICategoryRepository : ICrudRepository<Category> { }
public class CategoryRepository(ProductDbContext context) : CrudRepository<Category>(context), ICategoryRepository
{
    protected override IOrderedQueryable<Category> Order(IQueryable<Category> query) => query.OrderBy(x => x.CategoryId);
}
