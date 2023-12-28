using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.Core.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.Backend.Core.Abstract
{
    public interface IService<TEntity> where TEntity : BaseEntity
    {
        PagedList<TResult> GetAll<TResult>(DataTablesOptions model, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);

        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

        Task<Tuple<int, int, IEnumerable<TEntity>>> GetAllAsync(
            int page, int pageSize,
            Expression<Func<TEntity, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

        Task<TEntity> GetAsync(
           Expression<Func<TEntity, bool>>? expression = null,
           bool trackChanges = false,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);

        Task UpdateAsync(TEntity entity);

        Task RemoveAsync(TEntity entity);

        Task AddAsync(TEntity entity);

        Task AddRangeAsync(IEnumerable<TEntity> entities);
    }
}
