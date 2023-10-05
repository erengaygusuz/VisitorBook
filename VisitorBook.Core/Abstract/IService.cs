using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorBook.Core.Models;

namespace VisitorBook.Core.Abstract
{
    public interface IService<T> where T : BaseModel
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task AddAsync(T entity);
    }
}
