using PAW.DataAccess.MSSQL;
using PAW.Models;
namespace PAW.DataAccess.Repositories;
public interface IProductRepository : ICrudRepository<Product> { }
public class ProductRepository(ProductDbContext context) : CrudRepository<Product>(context), IProductRepository
{
    protected override IOrderedQueryable<Product> Order(IQueryable<Product> query) => query.OrderBy(x => x.ProductId);
}
