using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;

namespace VisitorBook.Core.Abstract
{
    public interface IUnitOfWork<T> where T : BaseModel
    {
        IRepository<T> Repository { get; }
        Task SaveAsync();
    }
}
