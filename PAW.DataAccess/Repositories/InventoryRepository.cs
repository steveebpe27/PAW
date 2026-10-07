using PAW.DataAccess.MSSQL;
using PAW.Models;
namespace PAW.DataAccess.Repositories;
public interface IInventoryRepository : ICrudRepository<Inventory> { }
public class InventoryRepository(ProductDbContext context) : CrudRepository<Inventory>(context), IInventoryRepository
{
    protected override IOrderedQueryable<Inventory> Order(IQueryable<Inventory> query) => query.OrderBy(x => x.InventoryId);
}
