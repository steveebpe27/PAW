using PAW.DataAccess.MSSQL;
using PAW.Models;
namespace PAW.DataAccess.Repositories;
public interface IUserRepository : ICrudRepository<User> { }
public class UserRepository(ProductDbContext context) : CrudRepository<User>(context), IUserRepository
{
    protected override IOrderedQueryable<User> Order(IQueryable<User> query) => query.OrderBy(x => x.UserId);
}
