using VisitorBook.Backend.Core.Entities;

namespace VisitorBook.Backend.Core.Abstract
{
    public interface IUnitOfWork<TEntity> where TEntity : BaseEntity
    {
        Task SaveAsync();
    }
}
