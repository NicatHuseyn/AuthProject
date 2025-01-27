using System.Linq.Expressions;
using System.Net;
using AuthProject.Core.Entities;
using AuthProject.Core.Repositories;
using AuthProject.Core.Services;
using AuthProject.Core.UnitOfWork;
using AuthProject.Service.Mapping;
using AuthProject.Shared;
using Microsoft.EntityFrameworkCore;

namespace AuthProject.Service.Services;

public class GenericService<TEntity, TDto>(IUnitOfWork unitOfWork, IGenericRepository<TEntity> repository) : IGenericService<TEntity, TDto> where TEntity : BaseEntity where TDto : class
{
    public async Task<Result<TDto>> AddAsync(TDto entity)
    {
        var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);

        await repository.AddAsync(newEntity);

        await unitOfWork.CommitAsync();

        var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);

        return Result<TDto>.Success(newDto,(int)HttpStatusCode.OK);
    }

    public Result<IEnumerable<TDto>> GetAll()
    {
        var products = ObjectMapper.Mapper.Map<List<TDto>>(repository.GetAll());
        return Result<IEnumerable<TDto>>.Success(products);
    }

    public async Task<Result<TDto>> GetByIdAsync(int id)
    {
        var data = await repository.GetByIdAsync(id);
        if (data is null)
            return Result<TDto>.Fail("Data Not Found", (int)HttpStatusCode.NotFound, true);

        var newDto = ObjectMapper.Mapper.Map<TDto>(data);

        return Result<TDto>.Success(newDto);
    }

    public async Task<Result<IEnumerable<TDto>>> GetWhereAsync(Expression<Func<TEntity, bool>> expression)
    {
        var datas = await repository.GetWhere(expression).ToListAsync();
        var dataAsDto = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(datas);

        return Result<IEnumerable<TDto>>.Success(dataAsDto);
    }

    public async Task<Result> RemoveAsync(int id)
    {
        var data = await repository.GetByIdAsync(id);
        if (data is null)
            return Result.Fail("Data Not Found", (int)HttpStatusCode.NotFound, true);

        repository.Remove(data);
        await unitOfWork.CommitAsync();

        return Result.Success((int)HttpStatusCode.OK);
    }

    public async Task<Result> UpdateAsync(TDto entity, int id)
    {
        var data = await repository.GetByIdAsync(id);
        if (data is null)
            return Result.Fail("Data Not Found", (int)HttpStatusCode.NotFound, true);

        var dataAsDto = ObjectMapper.Mapper.Map<TEntity>(entity);

        repository.Update(dataAsDto);

        await unitOfWork.CommitAsync();

        return Result.Success((int)HttpStatusCode.NoContent);

    }
}
