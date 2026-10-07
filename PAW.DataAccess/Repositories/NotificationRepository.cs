using PAW.DataAccess.MSSQL;
using PAW.Models;
namespace PAW.DataAccess.Repositories;
public interface INotificationRepository : ICrudRepository<Notification> { }
public class NotificationRepository(ProductDbContext context) : CrudRepository<Notification>(context), INotificationRepository
{
    protected override IOrderedQueryable<Notification> Order(IQueryable<Notification> query) => query.OrderBy(x => x.Id);
}
