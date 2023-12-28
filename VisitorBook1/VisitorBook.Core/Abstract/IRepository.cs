using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using VisitorBook.Core.Entities;

namespace VisitorBook.Core.Abstract
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetAsync(
           Expression<Func<TEntity, bool>>? expression = null,
           bool trackChanges = false,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAll(
            Expression<Func<TEntity, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

        Tuple<int, int, IQueryable<TEntity>> GetAll(
            int page, int pageSize,
            Expression<Func<TEntity, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

        void Remove(TEntity entity);

        Task AddAsync(TEntity entity);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
    }
}
