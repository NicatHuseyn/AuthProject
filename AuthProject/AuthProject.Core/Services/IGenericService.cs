using System.Linq.Expressions;
using AuthProject.Core.Entities;
using AuthProject.Shared;

namespace AuthProject.Core.Services;

public interface IGenericService<TEntity, TDto> where TEntity : BaseEntity where TDto : class
{
    Task<Result<IEnumerable<TDto>>> GetAllAsync(bool tracking = true);
    Task<Result<TDto>> GetByIdAsync(int id, bool tracking = true);
    Result<IEnumerable<TDto>> GetWhere(Expression<Func<TEntity, bool>> expression, bool tracking = true);

    Task<Result<TDto>> AddAsync(TEntity entity);

    Task<Result<IEnumerable<TDto>>> AddRangeAsync(IEnumerable<TEntity> entities);

    Task<Result> RemoveAsync(TEntity entity);

    Task<Result> UpdateAsync(TEntity entity);
}
