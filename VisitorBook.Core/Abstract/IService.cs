using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using VisitorBook.Core.Models;

namespace VisitorBook.Core.Abstract
{
    public interface IService<T> where T : BaseModel
    {
        Task<IEnumerable<T>> GetAllAsync(
           Expression<Func<T, bool>>? expression = null,
           bool trackChanges = false,
           Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<T> GetAsync(
           Expression<Func<T, bool>>? expression = null,
           bool trackChanges = false,
           Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task AddAsync(T entity);
    }
}
