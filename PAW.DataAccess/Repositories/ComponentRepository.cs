using PAW.DataAccess.MSSQL;
using PAW.Models;
namespace PAW.DataAccess.Repositories;
public interface IComponentRepository : ICrudRepository<Component> { }
public class ComponentRepository(ProductDbContext context) : CrudRepository<Component>(context), IComponentRepository
{
    protected override IOrderedQueryable<Component> Order(IQueryable<Component> query) => query.OrderBy(x => x.Id);
}
