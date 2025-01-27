using System.Linq.Expressions;
using AuthProject.Core.Entities;

namespace AuthProject.Core.Repositories;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    IQueryable<TEntity> GetAll(bool tracking = true);
    Task<TEntity> GetByIdAsync(int id, bool tracking = true);
    IQueryable<TEntity> GetWhere(Expression<Func<TEntity,bool>> expression, bool tracking = true);

    Task AddAsync(TEntity entity);

    Task AddRangeAsync(IEnumerable<TEntity> entities);

    void Remove(TEntity entity);

    TEntity Update(TEntity entity);
}
