using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VisitorBook.Backend.Core.Abstract;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.DAL.Data;

namespace VisitorBook.Backend.DAL.Concrete
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = _appDbContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> query = _appDbContext.Set<TEntity>();

            return query;
        }

        public IQueryable<TEntity> GetAll(
            Expression<Func<TEntity, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            IQueryable<TEntity> query = _appDbContext.Set<TEntity>();

            if (include != null)
            {
                query = include(query);
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (!trackChanges)
            {
                query.AsNoTracking();
            }

            return query;
        }

        public Tuple<int, int, IQueryable<TEntity>> GetAll(
            int page, int pageSize,
            Expression<Func<TEntity, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            IQueryable<TEntity> query = _appDbContext.Set<TEntity>();

            if (include != null)
            {
                query = include(query);
            }

            int totalCount = query.Count();
            int filteredCount = totalCount;

            if (expression != null)
            {
                query = query.Where(expression);
                filteredCount = query.Count();
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            if (!trackChanges)
            {
                query.AsNoTracking();
            }

            return new Tuple<int, int, IQueryable<TEntity>>(totalCount, filteredCount, query);
        }

        public async Task<TEntity> GetAsync(
            Expression<Func<TEntity, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            IQueryable<TEntity> query = _appDbContext.Set<TEntity>();

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (!trackChanges)
            {
                query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }
    }
}
