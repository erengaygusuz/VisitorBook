using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using VisitorBook.Core.Models;

namespace VisitorBook.Core.Abstract
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<T> GetAsync(
           Expression<Func<T, bool>>? expression = null,
           bool trackChanges = false,
           Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        IQueryable<T> GetAll();

        IQueryable<T> GetAll(
            Expression<Func<T, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

        Tuple<int, int, IQueryable<T>> GetAll(
            int page, int pageSize,
            Expression<Func<T, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

        void Remove(T entity);

        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T entity);
    }
}
