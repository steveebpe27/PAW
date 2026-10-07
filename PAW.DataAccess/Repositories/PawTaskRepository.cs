using PAW.DataAccess.MSSQL;
using PAW.Models;
namespace PAW.DataAccess.Repositories;
public interface IPawTaskRepository : ICrudRepository<PawTask> { }
public class PawTaskRepository(ProductDbContext context) : CrudRepository<PawTask>(context), IPawTaskRepository
{
    protected override IOrderedQueryable<PawTask> Order(IQueryable<PawTask> query) => query.OrderBy(x => x.Id);
}
