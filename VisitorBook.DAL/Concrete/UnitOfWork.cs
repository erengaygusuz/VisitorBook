using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
using VisitorBook.DAL.Data;

namespace VisitorBook.DAL.Concrete
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : BaseModel
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
