using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.Core.Abstract
{
    public interface IService<TEntity> where TEntity : BaseEntity
    {
        PagedList<TDto> GetAll<TDto>(DataTablesOptions model, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);

        Task<IEnumerable<TDto>> GetAllAsync<TDto>(
            Expression<Func<TEntity, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

        Task<Tuple<int, int, IEnumerable<TDto>>> GetAllAsync<TDto>(
            int page, int pageSize,
            Expression<Func<TEntity, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

        Task<TDto> GetAsync<TDto>(
           Expression<Func<TEntity, bool>>? expression = null,
           bool trackChanges = false,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);

        Task<bool> UpdateAsync<TDto>(TDto dto);

        Task<bool> RemoveAsync<TDto>(TDto dto);

        Task<bool> AddAsync<TDto>(TDto dto);

        Task<bool> AddRangeAsync<TDto>(IEnumerable<TDto> dto);
    }
}
