using System.Globalization;
using Microsoft.EntityFrameworkCore;
using PAW.DataAccess.MSSQL;
using PAW.Models.DTO;
namespace PAW.DataAccess.Repositories;
public interface ICrudRepository<T> where T : class
{
    Task<PageResult<T>> ReadPageAsync(int page, int pageSize);
    Task<List<T>> ReadAsync();
    Task<T?> FindAsync(string id);
    Task SaveAsync(T entity, bool isNew);
    Task DeleteAsync(T entity);
}
public abstract class CrudRepository<T>(ProductDbContext context) : ICrudRepository<T> where T : class
{
    protected ProductDbContext Context => context;
    protected abstract IOrderedQueryable<T> Order(IQueryable<T> query);
    public async Task<PageResult<T>> ReadPageAsync(int page, int pageSize)
    {
        pageSize = Math.Clamp(pageSize, 1, 25);
        var query = context.Set<T>().AsNoTracking();
        var count = await query.CountAsync();
        var totalPages = Math.Max(1, (int)Math.Ceiling(count / (double)pageSize));
        page = Math.Clamp(page, 1, totalPages);
        return new PageResult<T> { Page = page, PageSize = pageSize, TotalCount = count,
            Items = await Order(query).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync() };
    }
    public Task<List<T>> ReadAsync() => Order(context.Set<T>().AsNoTracking()).ToListAsync();
    public async Task<T?> FindAsync(string id)
    {
        // SQL keys here are positive integers, including Component's numeric(18,0).
        if (string.IsNullOrEmpty(id) || id.Length > 18 || id.Any(c => c < '0' || c > '9')) return null;
        var keyType = context.Model.FindEntityType(typeof(T))!.FindPrimaryKey()!.Properties.Single().ClrType;
        object key;
        if (keyType == typeof(int))
        {
            if (!int.TryParse(id, NumberStyles.None, CultureInfo.InvariantCulture, out var value) || value < 1) return null;
            key = value;
        }
        else
        {
            if (!decimal.TryParse(id, NumberStyles.None, CultureInfo.InvariantCulture, out var value) || value < 1) return null;
            key = value;
        }
        return await context.Set<T>().FindAsync(key);
    }
    public async Task SaveAsync(T entity, bool isNew)
    {
        if (isNew) context.Set<T>().Add(entity);
        await context.SaveChangesAsync();
    }
    public async Task DeleteAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }
}
