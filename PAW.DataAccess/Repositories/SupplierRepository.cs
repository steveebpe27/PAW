using PAW.DataAccess.MSSQL;
using PAW.Models;
namespace PAW.DataAccess.Repositories;
public interface ISupplierRepository : ICrudRepository<Supplier> { }
public class SupplierRepository(ProductDbContext context) : CrudRepository<Supplier>(context), ISupplierRepository
{
    protected override IOrderedQueryable<Supplier> Order(IQueryable<Supplier> query) => query.OrderBy(x => x.SupplierId);
}
