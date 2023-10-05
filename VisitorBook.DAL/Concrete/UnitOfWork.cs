using Microsoft.EntityFrameworkCore.Storage;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Models;
using VisitorBook.DAL.Data;

namespace VisitorBook.DAL.Concrete
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : BaseModel
    {
        private readonly AppDbContext _appDbContext;

        private readonly IDbContextTransaction _appDbContextTransaction;

        private readonly Lazy<IRepository<T>> _repository;

        public IRepository<T> Repository => _repository.Value;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

            _appDbContextTransaction = _appDbContext.Database.BeginTransaction();

            _repository = new Lazy<IRepository<T>>(
                () => new Repository<T>(_appDbContext)
            );
        }

        public async Task SaveAsync()
        {
            try
            {
                await _appDbContext.SaveChangesAsync();

                await _appDbContextTransaction.CommitAsync();
            }

            catch
            {
                await _appDbContextTransaction.RollbackAsync();

                throw;
            }
        }
    }
}
