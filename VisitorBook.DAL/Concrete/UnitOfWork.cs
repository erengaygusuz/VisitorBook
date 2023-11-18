using VisitorBook.Core.Abstract;
using VisitorBook.Core.Entities;
using VisitorBook.DAL.Data;

namespace VisitorBook.DAL.Concrete
{
    public class UnitOfWork<TEntity> : IUnitOfWork<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task SaveAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
