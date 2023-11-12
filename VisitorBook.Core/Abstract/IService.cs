using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using VisitorBook.Core.Models;
using VisitorBook.Core.Utilities.DataTablesServerSideHelpers;

namespace VisitorBook.Core.Abstract
{
    public interface IService<T> where T : BaseModel
    {
        PagedList<TResult> GetAll<TResult>(DataTablesOptions model, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

        Task<Tuple<int, int, IEnumerable<T>>> GetAllAsync(
            int page, int pageSize,
            Expression<Func<T, bool>>? expression = null,
            bool trackChanges = false,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

        Task<T> GetAsync(
           Expression<Func<T, bool>>? expression = null,
           bool trackChanges = false,
           Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task UpdateAsync(T entity);

        Task RemoveAsync(T entity);

        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);
    }
}
