using VisitorBook.Core.Entities;

namespace VisitorBook.Core.Abstract
{
    public interface IUnitOfWork<TEntity> where TEntity : BaseEntity
    {
        Task<bool> SaveAsync();
    }
}
