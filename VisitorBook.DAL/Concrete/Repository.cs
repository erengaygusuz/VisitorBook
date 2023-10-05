using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VisitorBook.DAL.Data;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;

namespace VisitorBook.DAL.Concrete
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = _appDbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<T> GetAll(
            Expression<Func<T, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _appDbContext.Set<T>();

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

            return query;
        }

        public async Task<T> GetAsync(
            Expression<Func<T, bool>> expression,
            bool trackChanges = false,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _appDbContext.Set<T>();

            query = query.Where(expression);

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

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
