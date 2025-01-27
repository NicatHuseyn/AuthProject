using System.Linq.Expressions;
using AuthProject.Core.Entities;
using AuthProject.Core.Repositories;
using AuthProject.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AuthProject.Data.Repositories;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : BaseEntity
{

    public DbSet<T> Table => context.Set<T>();

    public async Task AddAsync(T entity)
    {
        await Table.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await Table.AddRangeAsync(entities);
    }

    public IQueryable<T> GetAll(bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (tracking)
            query.AsNoTracking();
        return query;
    }

    public async Task<T> GetByIdAsync(int id, bool tracking = true)
    {
        var entity = await Table.FindAsync(id);
        var query = Table.AsQueryable();
        if (tracking)
            query.AsNoTracking();

        if (entity is not null)
        {
            Table.Entry(entity).State = EntityState.Detached;
        }
        return entity;
    }

    public IQueryable<T> GetWhere(Expression<Func<T, bool>> expression, bool tracking = true)
    {
        var query = Table.AsQueryable();
        if (tracking)
            query.AsNoTracking();
        return query.Where(expression);
    }

    public void Remove(T entity)
    {
        Table.Remove(entity);
    }

    public T Update(T entity)
    {
        Table.Entry(entity).State = EntityState.Modified;
        return entity;  
    }
}
