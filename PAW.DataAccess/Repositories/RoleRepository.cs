using PAW.DataAccess.MSSQL;
using PAW.Models;
namespace PAW.DataAccess.Repositories;
public interface IRoleRepository : ICrudRepository<Role> { }
public class RoleRepository(ProductDbContext context) : CrudRepository<Role>(context), IRoleRepository
{
    protected override IOrderedQueryable<Role> Order(IQueryable<Role> query) => query.OrderBy(x => x.RoleId);
}
