using VisitorBook.Core.Models;

namespace VisitorBook.Core.Abstract
{
    public interface IUnitOfWork<T> where T : BaseModel
    {
        Task SaveAsync();
    }
}
