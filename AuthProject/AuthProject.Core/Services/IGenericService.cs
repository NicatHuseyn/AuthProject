using System.Linq.Expressions;
using AuthProject.Core.Entities;
using AuthProject.Shared;

namespace AuthProject.Core.Services;

public interface IGenericService<TEntity, TDto> where TEntity : BaseEntity where TDto : class
{
    Result<IEnumerable<TDto>> GetAll();
    Task<Result<TDto>> GetByIdAsync(int id);
    Task<Result<IEnumerable<TDto>>> GetWhereAsync(Expression<Func<TEntity, bool>> expression);

    Task<Result<TDto>> AddAsync(TDto entity);


    Task<Result> RemoveAsync(int id);

    Task<Result> UpdateAsync(TDto entity, int id);
}
