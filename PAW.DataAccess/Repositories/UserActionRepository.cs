using PAW.DataAccess.MSSQL;
using PAW.Models;
namespace PAW.DataAccess.Repositories;
public interface IUserActionRepository : ICrudRepository<UserAction> { }
public class UserActionRepository(ProductDbContext context) : CrudRepository<UserAction>(context), IUserActionRepository
{
    protected override IOrderedQueryable<UserAction> Order(IQueryable<UserAction> query) => query.OrderBy(x => x.CrudId);
}
