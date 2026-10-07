using PAW.DataAccess.MSSQL;
using PAW.Models;
namespace PAW.DataAccess.Repositories;
public interface IUserRoleRepository : ICrudRepository<UserRole> { }
public class UserRoleRepository(ProductDbContext context) : CrudRepository<UserRole>(context), IUserRoleRepository
{
    protected override IOrderedQueryable<UserRole> Order(IQueryable<UserRole> query) => query.OrderBy(x => x.CrudId);
}
