using VisitorBook.Backend.Core.Abstract;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.DAL.Data;

namespace VisitorBook.Backend.DAL.Concrete
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
